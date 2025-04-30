using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Interfaces.Person;
using EduConnect.Entities;
using EduConnect.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EduConnect.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticationGuard(bool isAdmin = false, bool isStudent = false, bool isTutor = false) : Attribute, IAsyncAuthorizationFilter
    {
        public bool IsAdmin { get; set; } = isAdmin;
        public bool IsStudent { get; set; } = isStudent;
        public bool IsTutor { get; set; } = isTutor;
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authorizationHeader = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                context.Result = new UnauthorizedObjectResult("Missing authorization header");
                return;
            }

            Console.WriteLine("Authorization header: " + authorizationHeader);

            var token = authorizationHeader.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedObjectResult("Missing token");
                return;
            }

            Console.WriteLine("Token: " + token);

            var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();

            Console.WriteLine("Validating request token");

            var validationResult = await tokenService.ValidateToken(token);

            if (!validationResult)
            {
                context.Result = new UnauthorizedObjectResult("Invalid token");
                return;
            }

            foreach (var claim in context.HttpContext.User.Claims)
            {
                Console.WriteLine("Claim: " + claim.Type + " - " + claim.Value);
            }

            var nameIdentifierClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Console.WriteLine(nameIdentifierClaim);

            if (string.IsNullOrEmpty(nameIdentifierClaim.Value) || !Guid.TryParse(nameIdentifierClaim.Value, out var publicPersonId))
            {
                context.Result = new UnauthorizedObjectResult("Invalid token");
                return;
            }

            var databaseToken = await tokenService.GetTokenByPersonPublicId(publicPersonId);

            if (databaseToken == null)
            {
                context.Result = new UnauthorizedObjectResult("No database token found");
                return;
            }

            Console.WriteLine("Person public id:" + databaseToken.Person.PersonPublicId);

            var personRepository = context.HttpContext.RequestServices.GetRequiredService<IPersonRepository>();

            var userRoles = await personRepository.GetRolesByPersonId(databaseToken.Person.PersonId);

            Console.WriteLine("Writing out roles for user: " + databaseToken.Person.PersonId);
            Console.WriteLine("Number of records of user roles: " + userRoles.Count);
            foreach (var role in userRoles)
            {
                Console.WriteLine("Role - Name:" + role.Name + " - Id: " + role.Id);
            }

            bool isAuthorized = false;
            List<string> requiredRoles = new();

            if (IsAdmin)
            {
                requiredRoles.Add("ADMIN");
            }
            if (IsStudent)
            {
                requiredRoles.Add("STUDENT");
            }
            if (IsTutor)
            {
                requiredRoles.Add("TUTOR");
            }

            Console.WriteLine("Required roles: " + string.Join(", ", requiredRoles));

            isAuthorized = userRoles.Where(x => requiredRoles.Contains(x.NormalizedName)).Any();




            if (!isAuthorized)
            {
                Console.WriteLine("User does not have the required role(s): " + string.Join(", ", requiredRoles));
                context.Result = new ObjectResult(ApiResponse<object>.GetApiResponse(
                    "Access to this resource is restricted, please contact your administrator for further details", new { }))
                {
                    StatusCode = 403
                };
            }

            context.HttpContext.Items["PersonId"] = databaseToken.Person.PersonId;
            context.HttpContext.Items["Role"] = userRoles.FirstOrDefault()?.Name;


        }
    }
}