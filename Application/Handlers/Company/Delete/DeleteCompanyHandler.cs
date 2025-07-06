using Application.Commands.Company;
using Application.Notifications;
using Contracts;
using Entities.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Company.Delete
{
    public class DeleteCompanyHandler : INotificationHandler<CompanyDeletedNotification>
    {
        private readonly IRepositoryManager _repositoryManager;

        public DeleteCompanyHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task Handle(CompanyDeletedNotification notification,CancellationToken cancellationToken)
        {
            var company = await _repositoryManager.Company.GetCompany(notification.Id,notification.TrackChange);
            if (company is null)
                throw new CompanyNotFoundException(notification.Id);

            _repositoryManager.Company.DeleteCompany(company);
            await _repositoryManager.SaveAsync();
        }
    }
}
