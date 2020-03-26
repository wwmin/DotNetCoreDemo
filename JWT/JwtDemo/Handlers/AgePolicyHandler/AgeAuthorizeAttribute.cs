using JwtDemo.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtDemo.Handlers.AgePolicyHandler
{
    public class AgeAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = CONSTANT.POLICY_PREFIX_AGE;
        public AgeAuthorizeAttribute(int age) => Age = age;

        public int Age
        {
            get
            {
                if (int.TryParse(Policy.Substring(POLICY_PREFIX.Length), out var age))
                {
                    return age;
                }
                return default;
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}
