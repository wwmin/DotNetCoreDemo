using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtDemo.Handlers
{
    public class PermissionAuthorizationHandler:AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        public PermissionAuthorizationHandler()
        {

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            var primaryGroupSidClaim = context.User.FindFirst(p => p.Type == ClaimTypes.Role);
            return null;
        }
    }
}
