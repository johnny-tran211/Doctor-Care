using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public bool Notify { get; set; }
        public string Feedback { get; set; }
        public int PatientId { get; set; }
        public User Patient { get; set; }
        public int DoctorId { get; set; }
        public User Doctor { get; set; }
    }
}
