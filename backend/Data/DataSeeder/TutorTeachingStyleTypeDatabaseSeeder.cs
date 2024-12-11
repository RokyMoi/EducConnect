using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities.Reference;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.DataSeeder
{
    public class TutorTeachingStyleTypeDatabaseSeeder
    {
        private readonly DataContext _dataContext;

        public TutorTeachingStyleTypeDatabaseSeeder(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task SeedTutorTeachingStyleTypeDataToDatabase()
        {

            //Ensure there is no existing data in the TutorTeachingStyleType table, so there is no duplicate data entries
            if (!await _dataContext.TutorTeachingStyleType.AnyAsync())
            {
                //Define the TutorTeachingStyleType data, 4 objects (Visual, Auditory, Kinesthetic, Mixed, Inquiry-based, Collaborative, Lecture-based, Project-based)
                List<TutorTeachingStyleType> tutorTeachingStyleTypeList = new List<TutorTeachingStyleType>() {
                    new TutorTeachingStyleType{
                        Name = "Visual",
                        Description = "Uses visual aids such as diagrams, charts, and graphs to present information.",
                    },
                    new TutorTeachingStyleType{
                        Name = "Auditory",
                        Description = "Uses spoken language to present information.",
                    },
                    new TutorTeachingStyleType{
                        Name = "Kinesthetic",
                        Description = "Uses hands-on activities to present information.",
                    },
                    new TutorTeachingStyleType{
                        Name = "Mixed",
                        Description = "Uses a combination of visual, auditory, and kinesthetic methods to present information.",
                    },
                    new TutorTeachingStyleType{
                        Name = "Inquiry-based",
                        Description = "Encourages students to ask questions and explore topics in depth.",
                    },
                    new TutorTeachingStyleType{
                        Name = "Collaborative",
                        Description = "Encourages students to work together to solve problems and complete projects.",
                    },
                    new TutorTeachingStyleType{
                        Name = "Lecture-based",
                        Description = "Uses a lecture format to present information.",
                    },
                    new TutorTeachingStyleType{
                        Name = "Project-based",
                        Description = "Uses projects to present information.",
                    }
                };

                //Add the defined data to the database
                await _dataContext.TutorTeachingStyleType.AddRangeAsync(tutorTeachingStyleTypeList);
                await _dataContext.SaveChangesAsync();

            }
        }
    }
}