using Application.Queries.Company;
using Contracts;
using Entities.ApiResponse;
using Entities.Exceptions;
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
    public class GetComapnyHandler : IRequestHandler<GetComapnyQuery,CompanyDto>
    {
        private readonly IRepositoryManager _repository;
        public GetComapnyHandler(IRepositoryManager repositoryManager)
        {
            _repository = repositoryManager;
        }

        public async Task<CompanyDto> Handle(GetComapnyQuery getComapnyQuery,CancellationToken cancellationToken)
        {
            var company = await _repository.Company.GetCompany(getComapnyQuery.Id, getComapnyQuery.TrackChange);
            if (company is null)
                throw new CompanyNotFoundException(getComapnyQuery.Id);
            return company.Adapt<CompanyDto>();
        }
    }
}
