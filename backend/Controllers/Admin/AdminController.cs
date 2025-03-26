using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Interfaces;
using EduConnect.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers.Admin
{
    [ApiController]
    [Route("admin")]
    public class AdminController(
        IAdminRepository _adminRepository
    ) : ControllerBase
    {
        [HttpPost("data-seed")]
        public async Task<IActionResult> SeedData()
        {
            var dataExists = await _adminRepository.DataExists();
            if (dataExists)
            {
                return BadRequest("Data already exists");
            }
            var seedResult = await _adminRepository.SeedData();
            if (!seedResult)
            {
                return BadRequest("Failed to seed data");
            }

            return Ok("Data seeded successfully");
        }

        [HttpGet("users/all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminRepository.GetAllUsers();

            return Ok(users);
        }

    }
}