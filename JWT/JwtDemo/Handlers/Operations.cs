using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtDemo.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public static class Operations
    {
        public static PolicyRequirement Create = new PolicyRequirement { Name = "Create" };
        public static PolicyRequirement Read = new PolicyRequirement { Name = "Read" };
        public static PolicyRequirement Update = new PolicyRequirement { Name = "Update" };
        public static PolicyRequirement Delete = new PolicyRequirement { Name = "Delete" };
    }
}
