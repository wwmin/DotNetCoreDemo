using JwtDemo.Authorization.Jwt;
using JwtDemo.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtDemo.Handlers
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        /// <summary>
        /// 授权方式 (cookie, bearer, oauth, openid)
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        /// <summary>
        /// jwt服务
        /// </summary>
        private readonly IJwtAppService _jwtApp;

        /// <summary>
        /// 当前上下文
        /// </summary>
        IHttpContextAccessor _httpContextAccessor = null;
        /// <summary>
        /// 用户store
        /// </summary>
        private readonly UserStore _userStore;
        public PermissionAuthorizationHandler(IAuthenticationSchemeProvider schemes, IJwtAppService jwtApp, IHttpContextAccessor httpContextAccessor, UserStore userStore)
        {
            Schemes = schemes;
            _jwtApp = jwtApp;
            _httpContextAccessor = httpContextAccessor;
            _userStore = userStore;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
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
                    if (!await _jwtApp.IsCurrentActiveTokenAsync())
                    {
                        context.Fail();
                        return;
                    }
                    httpContext.User = result.Principal;

                    //判断是否过期
                    if (DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) < DateTime.UtcNow)
                    {
                        context.Fail();
                        return;
                    }
                }
                else
                {
                    context.Fail();
                    return;
                }
            }
            else
            {
                context.Fail();
                return;
            }

            var role = context.User.FindFirst(p => p.Type == ClaimTypes.Role);
            if (role != null)
            {
                if (context.User.IsInRole("admin"))
                {
                    context.Succeed(requirement);
                    return;
                }
                else
                {
                    var userIdClaim = context.User.FindFirst(p => p.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                        if (_userStore.CheckPermission(int.Parse(userIdClaim.Value), requirement.Name))
                        {
                            context.Succeed(requirement);
                            return;
                        }
                    }
                }
            }
            context.Fail();
        }
    }
}
