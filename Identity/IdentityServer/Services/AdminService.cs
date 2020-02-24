using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Services
{
    public interface IAdminService
    {
        Task<Admin> GetByStr(string username, string pwd);
    }

    public class AdminService : IAdminService
    {
        public EFContext db;

        public AdminService(EFContext _efContext)
        {
            db = _efContext;
        }

        public async Task<Admin> GetByStr(string username, string pwd)
        {
            Admin a = await db.Admin.Where(a => a.UserName == username && a.Password == pwd).SingleOrDefaultAsync();
            return a;
        }
    }
}
