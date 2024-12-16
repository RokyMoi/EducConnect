using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.DataSeeder
{
    public class EmploymentTypeDatabaseSeeder
    {
        private readonly DataContext _dataContext;

        public EmploymentTypeDatabaseSeeder(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task SeedEmploymentTypeDataToDatabase()
        {

            //Ensure there is no existing data in the EmploymentType table, so there is no duplicate data entries

            if (!await _dataContext.EmploymentType.AnyAsync())
            {
                //Define the EmploymentType data, 4 objects (Full-time, Part-time, Contract, Internship)
                List<EmploymentType> employmentTypeList = new List<EmploymentType>();

                //Create Full-time EmploymentType 
                employmentTypeList.Add(new EmploymentType
                {
                    Name = "Full-time",
                    Description = "Employed on a full-time basis.",
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    UpdatedAt = null
                });

                //Create Part-time EmploymentType
                employmentTypeList.Add(new EmploymentType
                {
                    Name = "Part-time",
                    Description = "	Employed on a part-time basis.",
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    UpdatedAt = null
                });

                //Create Contract EmploymentType

                employmentTypeList.Add(new EmploymentType
                {
                    Name = "Contract",
                    Description = "Employed under a specific contract term.",
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    UpdatedAt = null
                });

                //Create Freelance EmploymentType
                employmentTypeList.Add(new EmploymentType
                {
                    Name = "Freelance",
                    Description = "Works independently, not as a regular employee.",
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    UpdatedAt = null
                });
                //Create Internship EmploymentType
                employmentTypeList.Add(new EmploymentType
                {
                    Name = "Internship",
                    Description = "Temporary employment for gaining experience.",
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    UpdatedAt = null

                });


                //Add the above list to the database
                await _dataContext.EmploymentType.AddRangeAsync(employmentTypeList);
                await _dataContext.SaveChangesAsync();

            }
        }
    }
}