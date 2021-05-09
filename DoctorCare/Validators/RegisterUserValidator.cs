using DoctorCare.Entities;
using DoctorCare.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(DoctorCareApiContext doctorCareContext)
        {
            RuleFor(x => x.RoleId).Equal(1);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).NotEmpty();
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Confirm Password must equal to Password");
            RuleFor(x => x.Email).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            RuleFor(x => x.Email).Custom((value, context) => {
                var existEmail = doctorCareContext.Users.Any(x => x.Email == value);
                if (existEmail) {
                    context.AddFailure("Email", "That email address is taken");
                }
            });
        }
    }
}
