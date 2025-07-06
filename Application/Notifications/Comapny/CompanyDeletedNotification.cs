using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications
{
     public sealed record CompanyDeletedNotification(Guid Id,bool TrackChange) :INotification;    
    
}
