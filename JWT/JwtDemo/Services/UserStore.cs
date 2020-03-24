using JwtDemo.Handlers;
using JwtDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtDemo.Services
{
    public class UserStore
    {
        private static List<User> _users = new List<User>() {
   new User { Id=1, Name="admin", Password="111111", Role="admin", Email="admin@gmail.com", PhoneNumber="18800000000"},
   new User { Id=2, Name="alice", Password="111111", Role="user", Email="alice@gmail.com", PhoneNumber="18800000001",
       Permissions = new List<UserPermission> {
   new UserPermission { UserId = 1, PermissionName = Permissions.User },
   new UserPermission { UserId = 1, PermissionName = Permissions.Role }
      }
    },
   new User { Id=3, Name="bob", Password="111111", Role = "user", Email="bob@gmail.com", PhoneNumber="18800000002", Permissions = new List<UserPermission> {
    new UserPermission { UserId = 2, PermissionName = Permissions.UserRead },
    new UserPermission { UserId = 2, PermissionName = Permissions.RoleRead }
      }
    },
            };

        public bool CheckPermission(int userId, string permissionName)
        {
            var user = _users.Find(p => p.Id == userId);
            if (user == null) return false;
            return user.Permissions.Any(p => permissionName.StartsWith(p.PermissionName));
        }
    }
}


