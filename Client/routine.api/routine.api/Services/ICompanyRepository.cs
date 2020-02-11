using routine.api.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace routine.api.Services
{
    public interface ICompanyRepository
    {
        void AddCompany(Company company);
        void AddEmployee(Guid companyId, Employee employee);
        Task<bool> CompanyExistsAsync(Guid companyId);
        void DeleteCompany(Company company);
        void DeleteEmployee(Employee employee);
        Task<IEnumerable<Company>> GetCompaniesAsync();
        Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds);
        Task<Company> GetCompanyAsync(Guid companyId);
        Task<IEnumerable<Employee>> GetEmployeeAsync(Guid companyId);
        Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId);
        Task<bool> SaveAsync();
        void UpdateCompany(Company company);
        void UpdateEmployee(Employee employee);
    }
}