using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace routine.api.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
