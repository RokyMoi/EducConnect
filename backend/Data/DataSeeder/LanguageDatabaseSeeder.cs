using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference.Language;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.DataSeeder
{


    public class LanguageDatabaseSeeder
    {
        private readonly DataContext _dataContext;

        public LanguageDatabaseSeeder(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SeedLanguageDataToDatabase()
        {

            //Ensure there is no existing data in the Language table, so there is not duplicate data
            if (!await _dataContext.Language.AnyAsync())
            {
                //Define the Language data, 5 objects (English, Spanish, French, German, Serbo-croatian)
                List<Language> languageList = [
                    new(){
                        Name = "English",
                        Code = "eng",
                        IsRightToLeft = false

                    },
                    new(){
                        Name = "Spanish",
                        Code = "spa",
                        IsRightToLeft = false

                    },
                    new(){
                        Name = "French",
                        Code = "fra",
                        IsRightToLeft = false
                    },
                    new(){
                        Name = "German",
                        Code = "deu",
                        IsRightToLeft = false

                    },
                    new(){
                        Name = "Bosnian/Croatian/Serbian (Serbo-Croatian)",
                        Code = "hbs"
                    }
                ];

                //Add the defined data in the list to the database
                await _dataContext.AddRangeAsync(
                    languageList
                );
                await _dataContext.SaveChangesAsync();
            }
        }

    }
}