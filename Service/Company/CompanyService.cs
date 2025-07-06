using Contracts;
using Entities;
using Entities.ApiResponse;
using Entities.Exceptions;
using Mapster;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public CompanyService(IRepositoryManager repository, ILoggerManager logger)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<CompanyDto> CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = company.Adapt<Company>();
            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();
            var companyToReturn = companyEntity.Adapt<CompanyDto>();
            return companyToReturn;
        }

        public async Task<ApiBaseResponse> GetAllCompanies(bool trackChanges)
        {
            var companies = await _repository.Company.GetAllCompanies(trackChanges);
            var mappedCompanies = companies.Adapt<IEnumerable<CompanyDto>>();

            return new ApiOkResponse<IEnumerable<CompanyDto>>(mappedCompanies);
        }

        public async Task<ApiBaseResponse> GetCompany(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
            if (company is null)
                return new CompanyNotFoundResponse(companyId);
            var mappedCompany = company.Adapt<CompanyDto>();
            return new ApiOkResponse<CompanyDto>(mappedCompany);
        }

        public async Task<IEnumerable<CompanyDto>> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();
            var companyEntities = await _repository.Company.GetByIds(ids, trackChanges);
            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesToReturn = companyEntities.Adapt<IEnumerable<CompanyDto>>();
            return companiesToReturn;
        }

        public async Task<(IEnumerable<CompanyDto> companies, string ids)>
            CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = companyCollection.Adapt<List<Company>>();
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }

            await _repository.SaveAsync();

            var companyCollectionToReturn = companyEntities.Adapt<IEnumerable<CompanyDto>>();
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

            return (companies: companyCollectionToReturn, ids);
        }

        public async Task DeleteCompany(Guid companyId) 
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, false);
            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();
        }

        public async Task UpdateCompany(Guid companyid, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyid, trackChanges);
            companyForUpdate.Adapt(company);
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
        #endregion
    }
}
