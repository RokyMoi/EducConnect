using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;
namespace backend.Data.DataSeeder
{
    public class TutorRegistrationStatusDataSeeder
    {
        private readonly DataContext _dataContext;

        public TutorRegistrationStatusDataSeeder(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task SeedTutorRegistrationStatusDataToDatabase()
        {

            //Ensure there is no existing data in the TutorRegistrationStatus table, so there is no duplicate data entries
            if (!await _dataContext.TutorRegistrationStatus.AnyAsync())
            {
                //Define 9 objects where each object represents a different status of tutor registration completion

                //Status definitions:
                //1. Email and Password, User must enter a valid email and password 
                //2. Email Verification, User must verify their email address
                //3. Personal Information, User must enter their personal information
                //4. Education, User must enter their education information
                //5. Career, User must enter their career information
                //6. Weekly schedule, User must enter their weekly schedule
                //7. Tutoring and teaching information, User must enter information about their tutoring style and teaching preferences
                //8. Financial Information, User must enter their financial information
                //9. Registration Complete, User has completed their registration

                List<EduConnect.Entities.Tutor.TutorRegistrationStatus> statusList = new List<EduConnect.Entities.Tutor.TutorRegistrationStatus>();

                //Create Status 1: Email and Password
                statusList.Add(
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Email and Password",
                        Description = "Provide a valid email and password.",
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        UpdatedAt = null
                    }
                );

                //Create Status 2: Email Verification
                statusList.Add(
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Email Verification",
                        Description = "Verify your email address.",
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
                );

                //Create Status 3: Personal Information
                statusList.Add(
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Personal Information",
                        Description = "Provide your personal information.",
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),

                    });

                //Create Status 4: Education Information
                statusList.Add(
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Education",
                        Description = "Provide your education information.",
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),

                    }
                );

                //Create Status 5: Career 
                statusList.Add(
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Career",
                        Description = "Provide your career information.",
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
                );

                //Create Status 6: Weekly Schedule
                statusList.Add(
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Weekly Schedule",
                        Description = "Define your weekly work time and availability for communication with students.",
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
                );

                //Create Status 7: Tutoring and teaching information
                statusList.Add(
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Tutoring and Teaching Information",
                        Description = "Please let us know how would you like to teach your subjects and tutor students",
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
                );

                //Create Status 8: Financial Information
                statusList.Add(
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Financial Information",
                        Description = "Provide your financial information.",
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
                );

                //Create Status 9: Registration Complete
                statusList.Add(
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Registration Complete",
                        Description = "You have successfully competed your registration.",
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
                );


                //Add above list of statuses to the database
                await _dataContext.TutorRegistrationStatus.AddRangeAsync(statusList);
                await _dataContext.SaveChangesAsync();
                Console.WriteLine($"Successfully seeded {statusList.Count} Tutor Registration Statuses");
            }
        }
    }
}