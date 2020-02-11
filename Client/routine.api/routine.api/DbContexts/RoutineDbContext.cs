using Microsoft.EntityFrameworkCore;
using routine.api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace routine.api.DbContexts
{
    public class RoutineDbContext : DbContext
    {
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>().Property(x => x.Introduction).HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>().Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().HasOne(x => x.Company).WithMany(x => x.Employees).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);

            #region seed
            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = Guid.Parse("57D43861-DFA4-4CC5-8D52-5AEEC77B991B"),
                    Name = "Microsoft",
                    Introduction = "Great Company"
                },
                new Company
                {
                    Id = Guid.Parse("8C213E32-F4D8-429D-9947-612369979DA7"),
                    Name = "Google",
                    Introduction = "No Evil Company ..."
                },
                new Company
                {
                    Id = Guid.Parse("C2587F29-6832-41F6-B94F-454A230E5D4D"),
                    Name = "Alipapa",
                    Introduction = "Fubao Company"
                });
            #endregion
        }
    }
}
