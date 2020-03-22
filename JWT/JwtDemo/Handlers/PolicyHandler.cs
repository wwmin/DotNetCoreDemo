using JwtDemo.Authorization.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtDemo.Handlers
{
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement, IDocument>
    {
        /// <summary>
        /// 授权方式 (cookie, bearer, oauth, openid)
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        /// <summary>
        /// jwt服务
        /// </summary>
        private readonly IJwtAppService _jetApp;
        IHttpContextAccessor _httpContextAccessor = null;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="jwtApp"></param>
        /// <param name="httpContextAccessor"></param>
        public PolicyHandler(IAuthenticationSchemeProvider schemes, IJwtAppService jwtApp, IHttpContextAccessor httpContextAccessor)
        {
            Schemes = schemes;
            _jetApp = jwtApp;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 授权处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <param name="resource">基于资源的授权</param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement, IDocument resource)
        {

            var httpContext = _httpContextAccessor.HttpContext;


            //获取授权方式
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {

                //验证签发的用户信息
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                if (result.Succeeded)
                {
                    //判断是否为已停用的token
                    if (!await _jetApp.IsCurrentActiveTokenAsync())
                    {
                        context.Fail();
                        return;
                    }
                    httpContext.User = result.Principal;


                    //判断是否过期
                    if (DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) < DateTime.UtcNow)
                    {
                        context.Fail();
                    }


                    //判断角色
                    //var url = httpContext.Request.Path.Value.ToLower();
                    var role = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value; ;
                    if (context.User.IsInRole("admin"))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        //允许任何人创建或读取资源
                        if(requirement == Operations.Create || requirement == Operations.Read)
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            //只有资源的创建者才可以修改和删除
                            if(context.User.Identity.Name == resource.Creator)
                            {
                                context.Succeed(requirement);
                            }
                            else
                            {
                                context.Fail();
                            }
                        }
                    }
                    return;
                }
            }

            context.Fail();
        }
    }

    /// <summary>
    /// 测试菜单类
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// url
        /// </summary>
        public string Url { get; set; }
    }
}
