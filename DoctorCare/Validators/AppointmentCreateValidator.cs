using DoctorCare.Entities;
using DoctorCare.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Validators
{
    public class AppointmentCreateValidator : AbstractValidator<AppointmentCreate>
    {
        private readonly int _startTime = 8;
        private readonly int _endTime = 17;
        public AppointmentCreateValidator()
        {
            RuleFor(x => x.DoctorId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty()
                .Custom((value, context) => {
                    if (value.Date < DateTime.Now.Date) {
                        context.AddFailure("Date", "Date has greater than or equal current date");
                    }
                });
            RuleFor(x => x.Hour).NotEmpty();
            RuleFor(x => x.Hour).Custom((value, context) => {
                if (value < _startTime || value > _endTime) 
                {
                    context.AddFailure("Hour", $"outside working hours ({_startTime}h - {_endTime}h )");
                }
            });
            /*RuleFor(x => x.DoctorId).NotEmpty()
                .Custom((value, context) => {
                    var doctorExist = doctorCareApiContext.Users.Any(x => x.Id == value);
                    if (!doctorExist) 
                    {
                        context.AddFailure("DoctorId", "Doctor does not exist");
                    }
                });
            RuleFor(x => x.PatientId).NotEmpty()
                .Custom((value, context) =>
                {
                    var patientExist = doctorCareApiContext.Users.Any(x => x.Id == value);
                    if (!patientExist)
                    {
                        context.AddFailure("PatientId", "Patient does not exist");
                    }
                });*/
        }
    }
}
