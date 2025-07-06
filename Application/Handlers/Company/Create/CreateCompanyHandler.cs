using Application.Commands.Company;
using Contracts;
using Mapster;
using MediatR;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
namespace Application.Handlers.Company
{
    public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
    {
        private readonly IRepositoryManager _repositoryManager;

        public CreateCompanyHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = request.CompanyDto.Adapt<Entities.Company>();
            _repositoryManager.Company.CreateCompany(company);
            await _repositoryManager.SaveAsync();

            return company.Adapt<CompanyDto>();
        }
    }
}
