using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Models;

namespace IdentityServer
{
    public class EFContext:DbContext
    {
        public EFContext(DbContextOptions<EFContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Admin>().HasIndex(x => x.UserName).IsUnique();
        }
        #region 实体集
        public DbSet<Admin> Admin { get; set; }
        #endregion
    }
}
