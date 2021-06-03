using AutoMapper;
using DoctorCare.Authorization;
using DoctorCare.Entities;
using DoctorCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DoctorCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor, Patient")]
    public class AppointmentsController : ControllerBase
    {
        private readonly DoctorCareApiContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        public AppointmentsController(DoctorCareApiContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }
        [HttpGet("Doctors/{doctorId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAllDoctorAppointment(int doctorId) 
        {
            // Doctor get All his/her appointment
            var doctorExist = _context.Users.Any(x => x.Id == doctorId);
            if (!doctorExist) 
            {
                return NotFound();
            }


            var authorizationResult = _authorizationService.AuthorizeAsync(User, null, new AppointmentDataRequirement(doctorId)).Result;
            if (!authorizationResult.Succeeded) 
            {
                return Forbid();
            }


            var appointments = await _context.Appointments
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Include(x => x.Status)
                .Where(ap => ap.DoctorId == doctorId)
                .ToListAsync();

            var appointmentDtos = _mapper.Map<List<AppointmentDto>>(appointments);

            return Ok(appointmentDtos);
        }

        [HttpGet("Patients/{patientId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAllPatientAppointment(int patientId)
        {
            // Patients get All his/her appointment
            var patientExist = _context.Users.Any(x => x.Id == patientId);
            if (!patientExist)
            {
                return NotFound();
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(User, null, new AppointmentDataRequirement(patientId)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var appointments = await _context.Appointments
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Include(x => x.Status)
                .Where(ap => ap.PatientId == patientId)
                .ToListAsync();

            var appointmentDtos = _mapper.Map<List<AppointmentDto>>(appointments);

            return Ok(appointmentDtos);
        }

        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetDetail(int appointmentId) 
        {
            var appointment = await _context.Appointments
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.Id == appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(User, appointment, new AppointmentAllowedRequirement(false)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var appointmentDto = _mapper.Map<AppointmentDto>(appointment);

            return Ok(appointmentDto);
        }

        [HttpPut("UpdateStatus/{appointmentId}")]
        public async Task<IActionResult> Put(int appointmentId, [FromBody] AppointmentUpdateStatus appointmentDto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null) 
            {
                return NotFound();
            }

            if (appointmentDto.IsCancel)
            {
                var authorizationBothRolesResult = _authorizationService.AuthorizeAsync(User, appointment, new AppointmentAllowedRequirement(false)).Result;
                if (!authorizationBothRolesResult.Succeeded)
                {
                    return Forbid();
                }
                appointment.StatusId = (int)StatusEnum.Cancel;
            }
            else 
            {          
                var authorizationDoctorResult = _authorizationService.AuthorizeAsync(User, appointment, new AppointmentAllowedRequirement(true)).Result;
                if (!authorizationDoctorResult.Succeeded) 
                {
                    return Forbid();
                }
                if (appointment.StatusId == (int)StatusEnum.Done || appointment.StatusId == (int)StatusEnum.Cancel)
                {
                    return BadRequest("Appointment have been done or canceled. Cannot update status !!!");
                }
                appointment.StatusId += 1;

                if (appointment.StatusId == (int)StatusEnum.Done) 
                {
                    appointment.Feedback = appointmentDto.Feedback;
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Post([FromBody] AppointmentCreate appointmentDto) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var doctorExist = _context.Users.Any(x => x.Id == appointmentDto.DoctorId);
            if (!doctorExist)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var duplicateHour = _context.Appointments
                .Any(ap => ap.Hour == appointmentDto.Hour && ap.DoctorId == appointmentDto.DoctorId && ap.Date.Date == appointmentDto.Date.Date);
            if (duplicateHour) 
            {
                return BadRequest("Doctor is busy in this hour");
            }

            var appointment = _mapper.Map<Appointment>(appointmentDto);
            appointment.StatusId = (int)StatusEnum.Coming;
            appointment.Notify = true;
            appointment.PatientId = int.Parse(User.FindFirst(user => user.Type == ClaimTypes.NameIdentifier).Value);

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
