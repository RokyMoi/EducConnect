using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference.Learning;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.DataSeeder
{
    public class LearningDifficultyLevelDatabaseSeeder
    {
        private readonly DataContext _dataContext;

        public LearningDifficultyLevelDatabaseSeeder(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task SeedLearningDifficultyLevelDataToDatabase()
        {

            //Ensure there is no existing data in the LearningDifficultyLevel table, so there is no duplicate data entries
            if (!await _dataContext.LearningDifficultyLevel.AnyAsync())
            {

                //Define the LearningDifficultyLevel data, 4 objects (Beginner, Intermediate, Advanced, Expert)

                List<LearningDifficultyLevel> difficultyLevelsList = new List<LearningDifficultyLevel> {
                    new() {
                     Name = "Beginner",
                     Description = "Represents an entry-level course, suitable for people with no prior knowledge."

                    },
                    new() {
                        Name = "Intermediate",
                        Description = "For learners with basic knowledge, looking to build on foundational skills."
                    },
                    new() {
                        Name = "Advanced",
                        Description = "Meant for those with who aim to deepen their understanding and advance their skills of the given subject."
                    },
                    new (){
                        Name = "Expert",
                        Description = "For those who are experts in the given subject, but want a deeper understanding of the subject specific concepts."
                    }
                };

                //Add the defined data in the list to the database
                await _dataContext.LearningDifficultyLevel.AddRangeAsync(
                    difficultyLevelsList
                );
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}