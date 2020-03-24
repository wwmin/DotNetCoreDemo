using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtDemo.Models
{
    public class UserPermission
    {
        public int UserId { get; set; }
        public string PermissionName { get; set; }
    }
}
