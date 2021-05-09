using DoctorCare.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DoctorCare.Authorization
{
    public class ProfileHandler : AuthorizationHandler<ProfileRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProfileRequirement requirement, User resource)
        {
            var userId = context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (int.Parse(userId) == resource.Id) {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
