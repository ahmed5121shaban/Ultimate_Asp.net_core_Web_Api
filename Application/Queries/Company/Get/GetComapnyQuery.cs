using MediatR;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Company
{
    public sealed record GetComapnyQuery(bool TrackChange , Guid Id) : IRequest<CompanyDto>;
}
