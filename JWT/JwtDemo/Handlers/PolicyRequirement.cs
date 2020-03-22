using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtDemo.Handlers
{
    public class PolicyRequirement : OperationAuthorizationRequirement, IAuthorizationRequirement
    {
        // 微软已经定义该类型Requirement , 如果有其他定义可自行添加属性
        //  public string Name { get; set; }
    }
}
