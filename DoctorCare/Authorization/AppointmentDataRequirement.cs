using DoctorCare.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Authorization
{
    public class AppointmentDataRequirement : IAuthorizationRequirement
    {
        public int Id { get; }
        public AppointmentDataRequirement(int id)
        {
            Id = id;
        }
    }
}
