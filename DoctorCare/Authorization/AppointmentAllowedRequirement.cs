using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Authorization
{
    public class AppointmentAllowedRequirement : IAuthorizationRequirement
    {
        public bool OnlyDoctor { get; }
        public AppointmentAllowedRequirement(bool onlyDoctor)
        {
            OnlyDoctor = onlyDoctor;
        }
    }
}
