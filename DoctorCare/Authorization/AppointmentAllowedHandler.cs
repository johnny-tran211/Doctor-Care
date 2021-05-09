using DoctorCare.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DoctorCare.Authorization
{
    public class AppointmentAllowedHandler : AuthorizationHandler<AppointmentAllowedRequirement, Appointment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AppointmentAllowedRequirement requirement, Appointment resource)
        {
            var userExist = context.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value;
            if (int.Parse(userExist) == resource.DoctorId || int.Parse(userExist) == resource.PatientId) 
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
