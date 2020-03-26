using JwtDemo.Authorization.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtDemo.Handlers.AgePolicyHandler
{
    public class AgeAuthorizationHandler : AuthorizationHandler<AgeRequirement>
    {
        private readonly ILogger<AgeAuthorizationHandler> _log;
        /// <summary>
        /// jwt服务
        /// </summary>
        private readonly IJwtAppService _jwtApp;

        /// <summary>
        /// 当前上下文
        /// </summary>
        IHttpContextAccessor _httpContextAccessor = null;
        public AgeAuthorizationHandler(ILogger<AgeAuthorizationHandler> log, IJwtAppService jwtApp, IHttpContextAccessor httpContextAccessor)
        {
            _log = log;
            _jwtApp = jwtApp;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            //判断是否为已停用的token
            if (!await _jwtApp.IsCurrentActiveTokenAsync())
            {
                context.Fail();
                return;
            }

            _log.LogInformation("Evaluating authorization requirement for age = {age}", requirement.Age);
            var dateOfBirth = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);
            DateTime.TryParse(dateOfBirth.Value, out DateTime birth);
            if (birth != null && birth.Year > 1970)
            {
                int age = DateTime.Today.Year - birth.Year;
                if (age >= requirement.Age)
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            _log.LogInformation($"Current user's age claim does not satified the companyLevel authorization requirement {requirement.Age}");

            context.Fail();
            return;
        }
    }
}
