using DoctorCare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public virtual ICollection<Appointment> DoctorAppointments { get; set; }

    }
}
