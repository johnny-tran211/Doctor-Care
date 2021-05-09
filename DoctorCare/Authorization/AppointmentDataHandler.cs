using DoctorCare.Entities;
using DoctorCare.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DoctorCare.Authorization
{
    public class AppointmentDataHandler : AuthorizationHandler<AppointmentDataRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AppointmentDataRequirement requirement)
        {
            var id = context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (int.Parse(id) == requirement.Id) 
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
