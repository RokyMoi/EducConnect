using backend.Entities.Reference;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.DataSeeder;
public class WorkTypeDatabaseSeeder
{
    public readonly DataContext _dataContext;

    public WorkTypeDatabaseSeeder(DataContext dataContext)
    {
        this._dataContext = dataContext;
    }

    public async Task SeedWorkTypeDataToDatabase()
    {

        //Ensure there is no existing data in the WorkType table, so there is no duplicate data entries
        if (!await _dataContext.WorkType.AnyAsync())
        {

            //Define the WorkType data, 4 objects (Remote, Onsite, Hybrid, Work from home)

            List<WorkType> workTypeList = new List<WorkType>()
            {
            };

            //Create Remote WorkType to the WorkType list
            WorkType remoteWorkType = new WorkType()
            {
                Name = "Remote",
                Description = "Fully remote work from any location.",
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = null

            };


            //Create Onsite WorkType to the
            WorkType onsiteWorkType = new WorkType()
            {
                Name = "Onsite",
                Description = "Work performed entirely on premises.",
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = null
            };

            //Create Hybrid WorkType to the
            WorkType hybridWorkType = new WorkType()
            {
                Name = "Hybrid",
                Description = "Combination of remote and on-site work.",
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = null
            };

            //Create Work from home to the WorkType list
            WorkType homeWorkType = new WorkType()
            {

                Name = "Work from home",
                Description = "Work performed from primary location of residence of the employee.",
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = null
            };

            //Add the above created WorkType objects to the WorkType list

            workTypeList.Add(remoteWorkType);
            workTypeList.Add(onsiteWorkType);
            workTypeList.Add(hybridWorkType);
            workTypeList.Add(homeWorkType);

            //Add the list to the database
            await _dataContext.WorkType.AddRangeAsync(workTypeList);
            await _dataContext.SaveChangesAsync();
        }

    }
}

