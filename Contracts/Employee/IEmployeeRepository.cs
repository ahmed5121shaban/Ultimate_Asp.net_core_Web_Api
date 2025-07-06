using Entities;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        Task<PagedList<Entities.Employee>> GetEmployees(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
        Task<Entities.Employee> GetEmployee(Guid companyId, Guid id, bool trackChanges);
        void CreateEmployeeForCompany(Guid companyId, Entities.Employee employee);
        void DeleteEmployee(Entities.Employee employee);
    }
}
