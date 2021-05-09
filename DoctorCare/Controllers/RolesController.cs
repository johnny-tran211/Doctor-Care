using DoctorCare.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class RolesController : ControllerBase
    {
        private readonly DoctorCareApiContext _context;
        public RolesController(DoctorCareApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            return Ok(await _context.Roles.ToListAsync());
        }
    }
}
