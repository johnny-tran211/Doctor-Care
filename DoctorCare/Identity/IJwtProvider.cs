using DoctorCare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Identity
{
    public interface IJwtProvider
    {
        public string GenerateJwtToken(User user);
    }
}
