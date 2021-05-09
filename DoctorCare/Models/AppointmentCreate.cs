using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Models
{
    public class AppointmentCreate
    {
        public int Hour { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
    }
}
