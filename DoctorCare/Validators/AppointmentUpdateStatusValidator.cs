using DoctorCare.Entities;
using DoctorCare.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Validators
{
    public class AppointmentUpdateStatusValidator : AbstractValidator<AppointmentUpdateStatus>
    {
        public AppointmentUpdateStatusValidator()
        {
            RuleFor(x => x.DoctorId).NotEmpty();
        }
    }
}
