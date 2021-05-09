using AutoMapper;
using DoctorCare.Entities;
using DoctorCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare
{
    public class EasyCarProfile : Profile
    {
        public EasyCarProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.RoleName, map => map.MapFrom(user => user.Role.RoleName));

            CreateMap<AppointmentCreate, Appointment>();

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(x => x.DoctorName, map => map.MapFrom(ap => ap.Doctor.Name))
                .ForMember(x => x.PatientName, map => map.MapFrom(ap => ap.Patient.Name))
                .ForMember(x => x.StatusName, map => map.MapFrom(ap => ap.Status.Name));
        }
    }
}
