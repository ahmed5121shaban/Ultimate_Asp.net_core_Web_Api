using Application.Notifications;
using Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Company.Notifications
{
    public class CompanyDeletedNotificationHandler : INotificationHandler<CompanyDeletedNotification>
    {
        private readonly ILoggerManager _logger;

        public CompanyDeletedNotificationHandler(ILoggerManager logger)
        {
            _logger = logger;
        }
        public async Task Handle(CompanyDeletedNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogWarn($"Delete action for the company with id:{notification.Id} has occurred.");
            await Task.CompletedTask;
        }
    }
}
