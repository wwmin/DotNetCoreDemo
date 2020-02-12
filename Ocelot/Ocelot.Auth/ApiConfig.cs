using System.Collections.Generic;
using IdentityServer4.Models;

namespace Ocelot.Auth
{
    public class ApiConfig
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("OcelotApi","OcelotBasic的Api")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client()
                {
                    ClientId = "OcelotBasic",//客户端的标识,唯一的
                    ClientSecrets = new[]{new Secret("wwmin111".Sha256()) },//客户端密码,进行了加密
                    AllowedGrantTypes = GrantTypes.ClientCredentials,//授权方式,这里采用的时客户端认证模式,只要ClientId,以及ClientSecrets正确即可访问对应的AllowedScopes里面的api资源
                    AllowedScopes = new[]{"OcelotApi"}//定义这个客户端可以访问的Api资源数组,上面只有一个api

                }
            };
        }
    }
}