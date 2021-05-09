using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Authorization
{
    public class ProfileRequirement : IAuthorizationRequirement
    {
        public ProfileRequirement()
        {

        }
    }
}
