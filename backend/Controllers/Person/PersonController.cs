using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities.Person;
using EduConnect.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers.Person
{
    [ApiController]
    [Route("person")]
    public class PersonController(DataContext db, ITokenService _tokenService, IStudentRepository _studentRepo) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            // Retrieve the person's email
            var personEmail = await db.PersonEmail.Include(x => x.Person).Where(x => x.Email == login.Email)
                .FirstOrDefaultAsync();
            Console.WriteLine("PersonEmail: " + personEmail.Email);

            if (personEmail == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "User not found",
                    data = new { },
                    timestamp = DateTime.Now,

                });
            }



            // Retrieve the corresponding person and password details
            var personPassword = await db.PersonPassword
    .FirstOrDefaultAsync(x => x.PersonId == personEmail.PersonId);

            if (personPassword == null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Username or password invalid",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            // Hash the provided password using the same salt as the stored hash
            using var hmac = new HMACSHA512(personPassword.Salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            // Compare the computed hash with the stored hash
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != personPassword.Hash[i])
                {
                    return BadRequest(new
                    {
                        success = "false",
                        message = "Username or password invalid",
                        data = new { },
                        timestamp = DateTime.Now,
                    });
                }
            }
            string role = "";
            // Check if the user is a Student or Tutor by looking up the appropriate tables
            var student = await db.Student.FirstOrDefaultAsync(x => x.PersonId == personEmail.PersonId);
            var tutor = await db.Tutor.FirstOrDefaultAsync(x => x.PersonId == personEmail.PersonId);

            if (student != null)
            {
                role = "student";  // User is a student
            }
            else if (tutor != null)
            {
                role = "tutor";  // User is a tutor
            }
            else
            {
                return Unauthorized(new
                {
                    success = "error",
                    message = "Role undefined",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }


            var token = await _tokenService.CreateTokenAsync(personEmail);

            return
            Ok(
                new
                {
                    success = "true",
                    message = "Login succesfull",
                    data = new

            UserDTO
                    {
                        Email = personEmail.Email,
                        Token = token,
                        Role = role,
                    },
                    timestamp = DateTime.Now,
                });

        }

    }
}