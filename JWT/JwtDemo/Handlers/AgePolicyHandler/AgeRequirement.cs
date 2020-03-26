using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtDemo.Handlers.AgePolicyHandler
{
    public class AgeRequirement : IAuthorizationRequirement
    {
        public int Age { get; private set; }
        public AgeRequirement(int age)
        {
            Age = age;
        }
    }
}
