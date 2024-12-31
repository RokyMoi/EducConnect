using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities.Reference;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.DataSeeder
{
    public class CommunicationTypeDatabaseSeeder
    {

        public readonly DataContext _dataContext;

        public CommunicationTypeDatabaseSeeder(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task SeedCommunicationTypeDataToDatabase()
        {
            //Ensure there is no existing data in the CommunicationType table, so there is no duplicate data entries
            if (!await _dataContext.CommunicationType.AnyAsync())
            {

                //Define the CommunicationType data, 4 object (Email, Video calls, Audio calls, Chat messaging)

                List<CommunicationType> communicationTypes =
                [
                    new CommunicationType
                    {
                        Name = "Email",
                    },
                    new CommunicationType
                    {
                        Name = "Video calls",
                    },
                    new CommunicationType
                    {
                        Name = "Audio calls",
                    },
                    new    CommunicationType
                    {
                        Name = "Chat messaging",
                    },
                ];


                //Add the data to the database
                await _dataContext.CommunicationType.AddRangeAsync(communicationTypes);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}