using MediatR;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Company
{
    public sealed record CreateCompanyCommand(CompanyForCreationDto CompanyDto) :IRequest<CompanyDto>;
}
