using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities.Reference;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.DataSeeder
{
    public class EngagementMethodDatabaseSeeder
    {
        private readonly DataContext _dataContext;

        public EngagementMethodDatabaseSeeder(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task SeedEngagementMethodDataToDatabase()
        {

            //Ensure there is no existing data in the EngagementMethod table, so there is no duplicate data entries
            if (!await _dataContext.EngagementMethod.AnyAsync())
            {
                //Define the EngagementMethod data, 7 objects (Quizzes, Discussions, Hands-on Activities, Group projects, Case studies, Presentations, Role-playing exercises, Simulations)

                List<EngagementMethod> engagementMethods = new List<EngagementMethod>
                {
                    new EngagementMethod
                    {
                        Name = "Quizzes",
                        Description = "Quizzes are a form of assessment that involves answering a series of questions to test a student's knowledge on a specific topic or subject.",

                    },
                    new EngagementMethod
                    {
                        Name = "Discussions",
                        Description = "Discussions are a form of assessment that involves students engaging in a group discussion to explore a specific topic or subject.",

                    },
                    new EngagementMethod
                    {
                        Name = "Hands-on Activities",
                        Description = "Hands-on activities are a form of assessment that involves students engaging in practical activities to demonstrate their understanding of a specific topic or subject.",

                    },
                    new EngagementMethod
                    {
                        Name = "Group projects",
                        Description = "Group projects are a form of assessment that involves students working in groups to complete a project or task.",
                    },
                    new EngagementMethod
                    {
                        Name = "Case studies",
                        Description = "Case studies are a form of assessment that involves students analyzing a real-world scenario or case to demonstrate their understanding of a specific topic or subject.",
                    },
                    new EngagementMethod
                    {
                        Name = "Presentations",
                        Description = "Presentations are a form of assessment that involves students delivering a presentation on a specific topic or subject.",
                    },
                    new EngagementMethod
                    {
                        Name = "Role-playing exercises",
                        Description = "Role-playing exercises are a form of assessment that involves students acting out a role in a simulated situation to demonstrate their understanding of a specific topic or subject.",
                    },
                    new EngagementMethod
                    {
                        Name = "Simulations",
                        Description = "Simulations are a form of assessment that involves students participating in a simulated scenario to demonstrate their understanding of a specific topic or subject.",
                    }
                };

                //Add the defined data to the database
                await _dataContext.EngagementMethod.AddRangeAsync(engagementMethods);
                await _dataContext.SaveChangesAsync();


            }
        }
    }
}