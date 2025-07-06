using Shared.DataTransferObjects.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.Company
{
    public record CompanyForCreationDto
    {
        [Required(ErrorMessage = "Company Name is Required")]
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public IEnumerable<EmployeeForCreationDto>? Employees { get; set; }
    }
      
}
