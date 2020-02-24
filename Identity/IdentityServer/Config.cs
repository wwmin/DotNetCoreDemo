using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer
{
    public class Config
    {
        /// <summary>
        /// scopes define the resources in your system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> Ids()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };


        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "client",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                //scopes that client has access to
                AllowedScopes = {"api1"}
            },
            new Client
            {
                ClientId = "mvc",
                ClientName = "MVC Client",
                AllowedGrantTypes = GrantTypes.Implicit,//隐式方式
                RequireConsent=false,//如果不需要显示否同意授权 页面 这里就设置为false
                RedirectUris = { "http://localhost:5002/signin-oidc" },//登录成功后返回的客户端地址
                PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },//注销登录后返回的客户端地址

                AllowedScopes =//下面这两个必须要加
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            },
            new Client{
                ClientId = "js",
                ClientName = "JavaScript Client",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                RedirectUris = {"http://localhost:5003/callback.html"},
                PostLogoutRedirectUris = {"http://localhost:5003/index.html"},
                AllowedCorsOrigins = {"http://localhost:5003"},

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1"
                }
            }
        };
    }
}
