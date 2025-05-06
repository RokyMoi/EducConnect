using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services
{
    public class FullTextSearchEnableService
    {
        private readonly ILogger<FullTextSearchEnableService> _logger;

        public FullTextSearchEnableService(ILogger<FullTextSearchEnableService> logger)
        {
            _logger = logger;
        }

        public async Task FullTextSearchSetup(DataContext dataContext)
        {
            try
            {
                _logger.LogInformation("Setting up full text search for CourseLessonContent table...");
                //Get number of indexes for the CourseLessonContent table
                const string checkIndexSql = @"
                        SELECT COUNT(*)
                        FROM sys.fulltext_indexes fi
                        JOIN sys.objects o ON fi.object_id = o.object_id
                        WHERE o.name = 'CourseLessonContent'
                        ";

                int indexExists;

                var conn = dataContext.Database.GetDbConnection();
                await conn.OpenAsync();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = checkIndexSql;
                    var result = await command.ExecuteScalarAsync();
                    indexExists = Convert.ToInt32(result);
                }


                _logger.LogInformation("Number of indexes found for CourseLessonContent table: {indexExists}", indexExists);

                //If the index does not exist (i.e. indexExists == 0), then create the index
                if (indexExists == 0)
                {
                    const string createIndexSql = @"
                            IF NOT EXISTS (SELECT * FROM sys.fulltext_catalogs WHERE name = 'CourseLessonContentCatalog')
                            BEGIN
                                CREATE FULLTEXT CATALOG CourseLessonContentCatalog AS DEFAULT;
                            END;

                            CREATE FULLTEXT INDEX ON Course.CourseLessonContent(Content)
                            KEY INDEX IX_CourseLessonContent_RowGuid
                            ON CourseLessonContentCatalog;
                ";

                    _logger.LogInformation("No index found, executing SQL query to create index");
                    await dataContext.Database.ExecuteSqlRawAsync(createIndexSql);
                    _logger.LogInformation("Index created successfully");
                }
                else if (indexExists == -1)
                {
                    _logger.LogError("An error occurred in the setup of the full text search for CourseLessonContent table, indexExists: {indexExists}", indexExists);
                    throw new Exception("An error occurred in the setup of the full text search for CourseLessonContent table");
                }
                else
                {
                    _logger.LogInformation("Index already exists for CourseLessonContent table");
                }
            }
            catch (System.Exception ex)
            {

                _logger.LogError(ex, "Failed to setup full text search for CourseLessonContent table");
                throw;
            }

        }


    }
}
