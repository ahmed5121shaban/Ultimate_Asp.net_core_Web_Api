using Contracts.DataShaper;
using Contracts.Employee;
using Entities.LinkModels;
using Shared.DataTransferObjects;
using System.Dynamic;
using System.Net.Http.Headers;

namespace CompanyEmployees.Utility
{
    public class EmployeeLinks : IEmployeeLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<EmployeeDto> _dataShaper;

        public EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }
        public LinkResponse TryGenerateLinks
            (IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId, HttpContext httpContext)
        {
            var shapedEmployees = ShapeData(employeesDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedEmployees
                    (employeesDto, fields, companyId, httpContext, shapedEmployees);

            return ReturnShapedEmployees(shapedEmployees);
        }
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = httpContext.Items["AcceptHeaderMediaType"] as MediaTypeHeaderValue;
            return mediaType?.MediaType?.Contains("hateoas", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }
        private List<ExpandoObject> ShapeData(IEnumerable<EmployeeDto> employeesDto, string fields) =>
                _dataShaper.ShapeData(employeesDto, fields)
                .Select(e => e)
                .ToList();

        private LinkResponse ReturnShapedEmployees(List<ExpandoObject> shapedEmployees) =>
            new LinkResponse { ShapedEntities = shapedEmployees };

        private LinkResponse ReturnLinkdedEmployees(IEnumerable<EmployeeDto> employeesDto,
            string fields, Guid companyId, HttpContext httpContext, List<ExpandoObject> shapedEmployees)
        {
            var employeeDtoList = employeesDto.ToList();

            for (var index = 0; index < employeeDtoList.Count(); index++)
            {
                var employeeLinks =
                    CreateLinksForEmployee(httpContext, companyId, employeeDtoList[index].Id, fields);
                shapedEmployees[index].TryAdd("Links", employeeLinks);
            }

            var employeeCollection = new LinkCollectionWrapper<ExpandoObject>(shapedEmployees);
            var linkedEmployees = CreateLinksForEmployees(httpContext, employeeCollection);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
        }

        private LinkCollectionWrapper<ExpandoObject> CreateLinksForEmployees
            (HttpContext httpContext, LinkCollectionWrapper<ExpandoObject> employeesWrapper)
        {
            employeesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext,
            "GetEmployeesForCompany", values: new { }),
            "self",
            "GET"));
            return employeesWrapper;
        }

        private List<Link> CreateLinksForEmployee(HttpContext httpContext, Guid companyId,
                Guid id, string fields = "")
        {
            var links = new List<Link>
                    {
                        new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany",
                values: new { companyId, id, fields }),
                        "self",
                        "GET"),
                        new Link(_linkGenerator.GetUriByAction(httpContext,
                "DeleteEmployeeForCompany", values: new { companyId, id }),
                        "delete_employee",
                        "DELETE"),
                        new Link(_linkGenerator.GetUriByAction(httpContext,
                "UpdateEmployeeForCompany", values: new { companyId, id }),
                        "update_employee",
                        "PUT"),
                        new Link(_linkGenerator.GetUriByAction(httpContext,
                "PartiallyUpdateEmployeeForCompany", values: new { companyId, id }),
                        "partially_update_employee",
                        "PATCH")
                    };

            return links;
        }
    }
}
