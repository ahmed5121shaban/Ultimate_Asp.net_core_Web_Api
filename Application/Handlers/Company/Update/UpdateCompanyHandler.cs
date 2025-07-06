using Application.Commands.Company;
using Contracts;
using Entities.Exceptions;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Company.Update
{
    public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, Unit>
    {
        private readonly IRepositoryManager _repositoryManager;

        public UpdateCompanyHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _repositoryManager.Company.GetCompany(request.Id,true);
            if(company is null)
                throw new CompanyNotFoundException(request.Id);
            request.Company.Adapt(company);
            await _repositoryManager.SaveAsync();
            return Unit.Value;
        }
    }
}
