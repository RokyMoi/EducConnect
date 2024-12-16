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
                //Define 9 objects where each object represents a different status of tutor registration completion, each status has a state that defines is it skippable by the user or not

                //Status definitions:
                //1. Email and Password, User must enter a valid email and password, not skippable
                //2. Email Verification, User must verify their email address, not skippable
                //3. Phone Number, User can enter their phone number, skippable
                //3. Personal Information, User must enter their personal information, skippable
                //4. Education, User must enter their education information, skippable
                //5. Career, User must enter their career information,  skippable
                //6. Weekly schedule, User must enter their weekly schedule, not skippable
                //7. Tutoring and teaching information, User must enter information about their tutoring style and teaching preferences, skippable
                //8. Financial Information, User must enter their financial information, not skippable
                //9. Registration Complete, User has completed their registration, not skippable

                List<EduConnect.Entities.Tutor.TutorRegistrationStatus> statusList =
                [
                    //Create Status 1: Email and Password
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Email and Password",
                        Description = "Provide a valid email and password.",
                        IsSkippable = false,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        UpdatedAt = null
                    }
,
                    //Create Status 2: Email Verification
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Email Verification",
                        Description = "Verify your email address.",
                        IsSkippable = false,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
,
                    //Create Status 3: Phone Number
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Phone Number",
                        Description = "Provide your phone number.",
                        IsSkippable = true,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    },
                    //Create Status 4: Personal Information
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Personal Information",
                        Description = "Provide your personal information.",
                        IsSkippable = true,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),

                    },
                    //Create Status 5: Education Information
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Education",
                        Description = "Provide your education information.",
                        IsSkippable = true,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),

                    }
,
                    //Create Status 6: Career 
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Career",
                        Description = "Provide your career information.",
                        IsSkippable = true,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
,
                    //Create Status 7: Weekly Schedule
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Weekly Schedule",
                        Description = "Define your weekly work time and availability for communication with students.",
                        IsSkippable = false,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
,
                    //Create Status 8: Tutoring and teaching information
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Tutoring and Teaching Information",
                        Description = "Please let us know how would you like to teach your subjects and tutor students",
                        IsSkippable = true,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
,
                    //Create Status 9: Financial Information
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Financial Information",
                        Description = "Provide your financial information.",
                        IsSkippable = false,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
,
                    //Create Status 10: Registration Complete
                    new EduConnect.Entities.Tutor.TutorRegistrationStatus
                    {
                        Name = "Registration Complete",
                        Description = "You have successfully competed your registration.",
                        IsSkippable = false,
                        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    }
,
                ];


                //Add above list of statuses to the database
                await _dataContext.TutorRegistrationStatus.AddRangeAsync(statusList);
                await _dataContext.SaveChangesAsync();
                Console.WriteLine($"Successfully seeded {statusList.Count} Tutor Registration Statuses");
            }
        }
    }

}