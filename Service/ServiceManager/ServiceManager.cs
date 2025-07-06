using Contracts;
using Contracts;
using Contracts.DataShaper;
using Contracts.Employee;
using Entities;
using Entities.ConfigurationModels;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Service.Contracts.AuthenticationService;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        IDataShaper<EmployeeDto> dataShaper;
        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger,
            IDataShaper<EmployeeDto> dataShaper, IEmployeeLinks employeeLinks,
            UserManager<User> userManager, IOptions<JwtConfiguration> configuration)
        {
            _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, logger));
            _employeeService = new Lazy<IEmployeeService>(() =>
            new EmployeeService( repository:repositoryManager, logger:logger, employeeLinks:employeeLinks, dataShaper: dataShaper));
            _authenticationService = new Lazy<IAuthenticationService>
                (() => new AuthenticationService(logger, userManager, configuration));
        }

        public ICompanyService CompanyService => _companyService.Value;
        public IEmployeeService EmployeeService => _employeeService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}
