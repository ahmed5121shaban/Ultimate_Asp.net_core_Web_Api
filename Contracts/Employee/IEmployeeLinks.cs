using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Employee
{
    public interface IEmployeeLinks
    {
        LinkResponse TryGenerateLinks
            (IEnumerable<EmployeeDto> employeesDto,string fields, Guid companyId, HttpContext httpContext);
    }
}
