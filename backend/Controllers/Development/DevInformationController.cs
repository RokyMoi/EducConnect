using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces.Person;
using EduConnect.Data;
using EduConnect.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Controllers.Development
{
    [ApiController]
    [Route("dev/information")]
    public class DevInformationController(DataContext dataContext, IPersonRepository _personRepository) : ControllerBase
    {
        [HttpGet("users/all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await dataContext.Person
            .Include(
                x => x.PersonDetails
            )
            .Include(
                x => x.PersonEmail
            )
            .Include(
                x => x.UserRoles
            )
            .Include(
                x => x.PersonDetails.Country
            )
            .ToListAsync();

            var result = new List<object>();
            foreach (var x in users)
            {
                var roles = await _personRepository.GetRolesByPersonId(x.PersonId);
                var roleNames = roles.Select(r => r.Name).ToList(); // Extract just the role names

                result.Add(new
                {
                    PersonId = x.PersonId,
                    Name = string.Concat(!string.IsNullOrEmpty(x.PersonDetails.FirstName) ? x.PersonDetails.FirstName : "", " ", !string.IsNullOrEmpty(x.PersonDetails.LastName) ? x.PersonDetails.LastName : ""),
                    Email = string.IsNullOrEmpty(x.PersonEmail.Email) ? "" : x.PersonEmail.Email,
                    Username = string.IsNullOrEmpty(x.PersonDetails.Username) ? "" : x.PersonDetails.Username,
                    CountryId = x.PersonDetails.CountryOfOriginCountryId ?? Guid.Empty,
                    CountryName = x.PersonDetails.Country?.Name ?? "",
                    UserRoles = roleNames
                });
            }
            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Users retrieved successfully",
                    result
                )
            );
        }
    }
}