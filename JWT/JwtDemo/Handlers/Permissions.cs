using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtDemo.Handlers
{
    public static class Permissions
    {
        public const string User = "User";
        public const string UserCreate = "User.Create";
        public const string UserRead = "User.Read";
        public const string UserUpdate = "User.Update";
        public const string UserDelete = "User.Delete";
    }
}
