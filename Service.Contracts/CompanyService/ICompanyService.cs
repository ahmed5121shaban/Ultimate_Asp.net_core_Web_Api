using Entities.ApiResponse;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface ICompanyService
    {
        Task<ApiBaseResponse> GetAllCompanies(bool trackChanges);
        Task<ApiBaseResponse> GetCompany(Guid companyId, bool trackChanges);
        Task<CompanyDto> CreateCompany(CompanyForCreationDto company);
        Task<IEnumerable<CompanyDto>> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
        Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection);
        Task DeleteCompany(Guid companyId);
        Task UpdateCompany(Guid companyid, CompanyForUpdateDto companyForUpdate, bool trackChanges);
    }
}
