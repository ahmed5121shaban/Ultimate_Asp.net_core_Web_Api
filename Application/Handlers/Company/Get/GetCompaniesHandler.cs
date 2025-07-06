using Application.Queries.Company;
using Contracts;
using Mapster;
using MediatR;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Company
{
    internal sealed class GetCompaniesHandler : IRequestHandler<GetCompaniesQuery,IEnumerable<CompanyDto>>
    {
        private readonly IRepositoryManager _repository;
        public GetCompaniesHandler(IRepositoryManager repository) => _repository = repository;
        public async Task<IEnumerable<CompanyDto>> Handle(GetCompaniesQuery request,CancellationToken cancellationToken)
        {
            var companies = await _repository.Company.GetAllCompanies(request.TrackChanges);
            var companiesDto = companies.Adapt<IEnumerable<CompanyDto>>();
            return companiesDto;
        }
    }
}
