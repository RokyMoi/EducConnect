using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Course;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.DataSeeder
{
    public class CourseTypeDatabaseSeeder
    {
        private readonly DataContext _dataContext;

        public CourseTypeDatabaseSeeder(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task SeedCourseTypeDataToDatabase()
        {

            //Ensure there is no existing data in the CourseType table, so there is no duplicate data entries
            if (!await _dataContext.CourseType.AnyAsync())
            {

                //Define the CourseType data, 2 objects (Self-paced course, Guided course)

                List<CourseType> courseTypeList = new List<CourseType>() {
                    new(){
                        Name = "Self-paced course",
                        Description = "Course where students using provided resources to learn at their own pace.",

                    },
                    new() {
                        Name = "Guided course",
                        Description = "Course where students toghether with a tutor, learn and tackle different topics of a subject together.",
                    }
                };

                //Add the CourseType data to the database
                await _dataContext.CourseType.AddRangeAsync(courseTypeList);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}