using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapWeb1.DbContext
{
    public class MyDBContext : Microsoft.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="options"></param>
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
