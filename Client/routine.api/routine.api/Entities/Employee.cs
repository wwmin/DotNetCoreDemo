using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace routine.api.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public Company Company { get; set; }
    }
}
