using AutoMapper;
using DoctorCare.Authorization;
using DoctorCare.Entities;
using DoctorCare.Identity;
using DoctorCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DoctorCareApiContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        public UsersController(DoctorCareApiContext context, IPasswordHasher<User> passwordHasher, IJwtProvider jwtProvider, IMapper mapper, IAuthorizationService authorizationService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var user = await _context.Users
                .Include(user => user.Role)
                .FirstOrDefaultAsync(user => user.Email == userLoginDto.Email);
            if (user == null)
            {
                return new ObjectResult("Invalid username or password") { StatusCode = StatusCodes.Status401Unauthorized };
            }

            var passwordVerficationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDto.Password);
            if (passwordVerficationResult == PasswordVerificationResult.Failed)
            {
                return new ObjectResult("Invalid username or password") { StatusCode = StatusCodes.Status401Unauthorized };
            }
            var token = _jwtProvider.GenerateJwtToken(user);
            return Ok(token);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newUser = new User()
            {
                Email = registerUserDto.Email,
                RoleId = registerUserDto.RoleId,
                Name = registerUserDto.Name
            };

            var passwordHash = _passwordHasher.HashPassword(newUser, registerUserDto.Password);
            newUser.PasswordHash = passwordHash;

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpGet("Doctors")]
        public async Task<IActionResult> GetDoctors() 
        {
            var doctors = await _context.Users.Include(x => x.Role).Include(x => x.DoctorAppointments).Where(u => u.RoleId == 2).ToListAsync();
            var doctorDtos = _mapper.Map<List<UserDto>>(doctors);
            return Ok(doctorDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(int id) 
        {
            var profile = await _context.Users.Include(x => x.Role).Include(x => x.DoctorAppointments).FirstOrDefaultAsync(x => x.Id == id);
            if (profile == null) 
            {
                return NotFound();
            }
            var authorizationResult = _authorizationService.AuthorizeAsync(User, profile, new ProfileRequirement()).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            var profileDto = _mapper.Map<UserDto>(profile);
            return Ok(profileDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, UpdateUserDto userDto) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var profile = await _context.Users.Include(x => x.Role).Include(x => x.DoctorAppointments).FirstOrDefaultAsync(x => x.Id == id);
            if (profile == null) 
            {
                return NotFound();
            }
            var authorizationResult = _authorizationService.AuthorizeAsync(User, profile, new ProfileRequirement()).Result;
            if (!authorizationResult.Succeeded) 
            {
                return Forbid();
            }
            profile.Address = userDto.Address;
            profile.DateOfBirth = userDto.DateOfBirth;
            profile.Name = userDto.Name;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
