using Contracts;
using Contracts;
using Entities.Exceptions;
using Entities;
using Mapster;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransferObjects.Employee;
using Shared.RequestFeatures;
using Entities.Exceptions.MaxAgeRangeBadRequestExceptions;
using Service.DataShaping;
using Contracts.DataShaper;
using System.Dynamic;
using Entities.LinkModels;
using System.Net.Http;
using System.Net.Http.Headers;
using Contracts.Employee;
using Microsoft.AspNetCore.Http;

namespace Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IDataShaper<EmployeeDto> _dataShaper;
        private readonly IEmployeeLinks _employeeLinks;

        public EmployeeService(IEmployeeLinks employeeLinks,IRepositoryManager repository, ILoggerManager logger, IDataShaper<EmployeeDto> dataShaper)
        {
            _logger = logger;
            _repository = repository;
            _dataShaper = dataShaper;
            _employeeLinks = employeeLinks;
        }

        public async Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            var employeeEntity = employeeForCreation.Adapt<Employee>();

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();

            var employeeToReturn = employeeEntity.Adapt<EmployeeDto>();

            return employeeToReturn;
        }

        public async Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

            _repository.Employee.DeleteEmployee(employeeEntity);
            await _repository.SaveAsync();
        }

        public async Task<EmployeeDto> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, employeeId, trackChanges);

            return employeeEntity.Adapt<EmployeeDto>();
        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync
                (Guid companyId, LinkParameters linkParameters, bool trackChanges)
        {
            if (!linkParameters.EmployeeParameters.ValidAgeRange)
                throw new MaxAgeRangeBadRequestException();

            await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            var employeesWithMetaData = await _repository.Employee.GetEmployees
                (companyId, linkParameters.EmployeeParameters,trackChanges);

            var employeesDto = employeesWithMetaData.Adapt<IEnumerable<EmployeeDto>>();

            var links = _employeeLinks.TryGenerateLinks
                (employeesDto,linkParameters.EmployeeParameters.Fields, companyId, linkParameters.Context);

            return (linkResponse: links, metaData: employeesWithMetaData.MetaData);
        }

        public async Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
        {
            await GetCompanyAndCheckIfItExists(companyId, compTrackChanges);

            var employee = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);
            employeeForUpdate.Adapt(employee);
            await _repository.SaveAsync();

        }

        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> 
            GetEmployeeForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            await GetCompanyAndCheckIfItExists(companyId, compTrackChanges);

            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

            var employeeToPatch = employeeEntity.Adapt<EmployeeForUpdateDto>();

            return (employeeToPatch, employeeEntity);
        }

        public async Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            employeeToPatch.Adapt(employeeEntity);
            await _repository.SaveAsync();
        }

            #region Utility
            private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompany(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            return company;
        }

        private async Task<Employee> 
            GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
        {
            var employeeDb = await _repository.Employee.GetEmployee(companyId, id,
           trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);

            return employeeDb;
        }
        #endregion
    }
}
