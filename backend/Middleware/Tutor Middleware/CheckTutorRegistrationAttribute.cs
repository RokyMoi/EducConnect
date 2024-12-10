using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.Entities.Person;
using EduConnect.Entities.Tutor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace backend.Middleware.Tutor
{
    public class CheckTutorRegistrationAttribute : ActionFilterAttribute
    {



        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //Get Email from claims
            var claimsEmail = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

            if (string.IsNullOrEmpty(claimsEmail))
            {
                context.Result = new UnauthorizedObjectResult(

                        new
                        {
                            success = "false",
                            message = "Please login first",
                            data = new { },
                            timestamp = DateTime.Now
                        }

                );
                return;
            }

            var serviceProvider = context.HttpContext.RequestServices;

            //Initialize document scope DataContext instance 

            var dataContext = context.HttpContext.RequestServices.GetRequiredService<DataContext>();

            //Get the person email along with tutor and student with the same personId from the database

            var personEmailWithTutorAndStudent = await dataContext.PersonEmail.
            Where(x => x.Email == claimsEmail)
            .GroupJoin(
                dataContext.Tutor,
                ppe => ppe.PersonId,
                t => t.PersonId,
                (ppe, tutorGroup) => new { ppe, tutorGroup }
            )
            .SelectMany(
                x => x.tutorGroup.DefaultIfEmpty(),
                (x, tt) => new { x.ppe, tt }
            )
            .GroupJoin(
                dataContext.Student,
                x => x.ppe.PersonId,
                sts => sts.PersonId,
                (x, studentGroup) => new { x.ppe, x.tt, studentGroup }
            )
            .SelectMany(
                x => x.studentGroup.DefaultIfEmpty(),
                (x, sts) => new { PersonEmail = x.ppe, Tutor = x.tt, Student = sts }
            ).FirstOrDefaultAsync();

            if (personEmailWithTutorAndStudent == null)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    success = "false",
                    message = "Account not found, please sign up first",
                    data = new { },
                    timestamp = DateTime.Now
                });
                return;
            }
            Console.WriteLine("Email to check: " + claimsEmail);
            Console.WriteLine("PersonEmail.PersonId: " + personEmailWithTutorAndStudent.PersonEmail.PersonId);
            Console.WriteLine("Tutor.PersonId: " + personEmailWithTutorAndStudent.Tutor?.PersonId);
            Console.WriteLine("Student.PersonId: " + personEmailWithTutorAndStudent.Student?.PersonId);

            //Check if the tutor registration status is valid (ie. not null)
            if (personEmailWithTutorAndStudent.Tutor.TutorRegistrationStatusId == null)
            {
                context.Result = new JsonResult(new
                {
                    success = "false",
                    message = "An error occurred while processing your request. Please try again later.",
                    data = new { },
                    timestamp = DateTime.Now
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                return;
            }

            //Check the tutor registration status
            if (personEmailWithTutorAndStudent.Tutor.PersonId != null)
            {

                var allTutorRegistrationStatusList = await dataContext.
                TutorRegistrationStatus
                .Select(
                    x => new
                    {
                        x.TutorRegistrationStatusId,
                        x.Name,
                        x.Description
                    }
                )
                .ToListAsync();

                //Check if the tutor registration status is completed 
                if (personEmailWithTutorAndStudent.Tutor.TutorRegistrationStatusId != allTutorRegistrationStatusList.Max(x => x.TutorRegistrationStatusId))
                {
                    context.Result = new JsonResult(
                        new
                        {
                            success = "false",
                            message = "It looks like you haven't completed your tutor registration yet. Please complete it to continue.",
                            data = new
                            {
                                CurrentTutorRegistrationStatus = new
                                {
                                    TutorRegistrationStatusId = personEmailWithTutorAndStudent.Tutor.TutorRegistrationStatusId,
                                    Name = allTutorRegistrationStatusList.FirstOrDefault(x => x.TutorRegistrationStatusId == personEmailWithTutorAndStudent.Tutor.TutorRegistrationStatusId)?.Name,
                                    Description = allTutorRegistrationStatusList.FirstOrDefault(x => x.TutorRegistrationStatusId == personEmailWithTutorAndStudent.Tutor.TutorRegistrationStatusId)?.Description
                                }
                            },
                            timestamp = DateTime.Now
                        }
                    )
                    {
                        StatusCode = StatusCodes.Status422UnprocessableEntity
                    };

                    return;

                }





            }
            await next();
        }

    }


}