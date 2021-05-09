using DoctorCare.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Validators
{
    public class UpdateUserValidator :AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEmpty()
                .Custom((value, context) => {
                    if (value.Value.AddYears(18) > DateTime.Now) {
                        context.AddFailure("DateOfBirth", "You must be over 18 years old");
                    }
                });
        }
    }
}
