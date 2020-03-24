using JwtDemo.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtDemo.Handlers
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly UserStore _userStore;
        public PermissionAuthorizationHandler(UserStore userStore)
        {
            _userStore = userStore;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            var role = context.User.FindFirst(p => p.Type == ClaimTypes.Role);
            if (role != null)
            {
                if (context.User.IsInRole("admin"))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    var userIdClaim = context.User.FindFirst(p => p.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                        if (_userStore.CheckPermission(int.Parse(userIdClaim.Value), requirement.Name))
                        {
                            context.Succeed(requirement);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
