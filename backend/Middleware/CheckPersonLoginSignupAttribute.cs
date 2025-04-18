using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.Interfaces;
using EduConnect.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace backend.Middleware
{
    public class CheckPersonLoginSignupAttribute : Attribute, IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var serviceProvider = context.HttpContext.RequestServices;


            //Get TokenService as a required service, to use it's method of token validation
            var jwtValidator = serviceProvider.GetService(typeof(ITokenService)) as ITokenService;

            Console.WriteLine("TokenService: " + jwtValidator);
            if (jwtValidator == null || serviceProvider == null)
            {
                context.Result = new JsonResult(new
                {
                    success = "error",
                    message = "Something went wrong, please try again later.",
                    data = new { },
                    timestamp = DateTime.Now
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                return;

            }

            //Get the Bearer type token from the request header field Authorization
            var token = context.HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();


            //If token is null, return 401
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedObjectResult(
                    new
                    {
                        success = false,
                        message = "You are not logged in.",
                        data = new
                        {
                            redirectionLink = "/login",
                        },
                        timestamp = DateTime.Now,
                    }
                );
            }

            //Validate the token
            var claimsPrincipal = jwtValidator.ValidateTokenWithClaims(token);
            if (claimsPrincipal == null)
            {
                context.Result = new UnauthorizedObjectResult(
                    new
                    {
                        success = false,
                        message = "Please log in again.",
                        data = new
                        {
                            redirectionLink = "/login",
                        },
                        timestamp = DateTime.Now,
                    }
                );
            }

            context.HttpContext.User = claimsPrincipal;

            Console.WriteLine("Is Authenticated: " + context.HttpContext.User?.Identity?.IsAuthenticated);

            //Check if the user is authenticated or the user email from token is not null
            if (!context.HttpContext.User.Identity.IsAuthenticated || string.IsNullOrEmpty(context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).ToString()))
            {
                context.Result = new UnauthorizedObjectResult(
                    new
                    {
                        success = false,
                        message = "You are not logged in.",
                        data = new
                        {
                            redirectionLink = "/login",
                        },
                        timestamp = DateTime.Now,
                    }
                );
                return;
            }

            //Check if the user is a student or a tutor
            //Set scope instance of Database Context 
            var dataContext = context.HttpContext.RequestServices.GetRequiredService<DataContext>();

            if (dataContext == null)
            {
                context.Result = new JsonResult(new
                {
                    success = "error",
                    message = "Something went wrong, please try again later.",
                    data = new { },
                    timestamp = DateTime.Now
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
                return;
            }

            //Get the Person data from the database using claims email
            string claimEmail = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value.ToString();

            Console.WriteLine("Claim email: " + claimEmail);
            var userData = await dataContext.PersonEmail.Where(x => x.Email == claimEmail).FirstOrDefaultAsync();

            if (userData == null)
            {
                context.Result = new JsonResult(
                    new
                    {
                        success = "false",
                        message = "Account not found, please sign up first",
                        data = new
                        {
                            redirectionLink = "/signup",
                        },
                        timestamp = DateTime.Now,
                    }
                )
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                };
                return;

            }



            //Set the email and personId into the context dictionary 
            context.HttpContext.Items.Add("Email", claimEmail);
            context.HttpContext.Items.Add("PersonId", userData.PersonId);


            Console.WriteLine("Email from dictionary: " + context.HttpContext.Items["Email"]);
            Console.WriteLine("PersonId from dictionary: " + context.HttpContext.Items["PersonId"]);

            await next();
        }
    }

}