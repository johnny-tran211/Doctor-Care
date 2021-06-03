using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Models
{
    public class AppointmentUpdateStatus
    {
        public int Id { get; set; }
        public string Feedback { get; set; }
        public int DoctorId { get; set; }
        public bool IsCancel { get; set; } = false;
    }
}
