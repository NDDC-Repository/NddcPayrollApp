using FluentValidation;
using NddcPayrollLibrary.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Validators
{
    public class EmployeeValidator : AbstractValidator<EmployeeModel>
    {
        public EmployeeValidator()
        {
            RuleFor(e => e.EmployeeCode).NotEmpty();
            RuleFor(e => e.Gender).NotEmpty();
            RuleFor(e => e.MaritalStatus).NotEmpty();
            RuleFor(e => e.FirstName).NotEmpty();
            RuleFor(e => e.LastName).NotEmpty();
            RuleFor(e => e.DateOfBirth).LessThan(DateTime.Today);
            RuleFor(e => e.SID).NotEmpty().WithMessage("You Must Select A State");
            RuleFor(e => e.EmploymentStatus).NotEmpty();
            RuleFor(e => e.DateEngaged).LessThanOrEqualTo(DateTime.Today);
        }

        protected bool BeAValidDate(DateTime? date)
        {
            if (date.Equals(DateTime.Now.ToShortDateString()))
            {
                return false;
            }
            return true;
        }

    }
}
