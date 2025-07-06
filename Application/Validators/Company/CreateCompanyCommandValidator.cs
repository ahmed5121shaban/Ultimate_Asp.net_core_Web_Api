using Application.Commands.Company;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Company
{
    public sealed class CreateCompanyCommandValidator: AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(c => c.CompanyDto.Name).NotEmpty().MaximumLength(60);
            RuleFor(c => c.CompanyDto.Address).NotEmpty().MaximumLength(60);
        }
    }
}
