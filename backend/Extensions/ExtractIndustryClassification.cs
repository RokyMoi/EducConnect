using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference;
using backend.Interfaces.Reference;
using Newtonsoft.Json;

namespace backend.Extensions
{
    public class ExtractIndustryClassification
    {
        private readonly IReferenceRepository _referenceRepository;

        public ExtractIndustryClassification(IReferenceRepository referenceRepository)
        {
            this._referenceRepository = referenceRepository;
        }

        public static List<IndustryClassification> ExtractIndustryClassificationFromJsonFile(string jsonFilePath)
        {
            string json = File.ReadAllText(jsonFilePath);

            var industryClassifications = JsonConvert.DeserializeObject<List<dynamic>>(json);

            var industryClassificationList = new List<IndustryClassification>();

            foreach (var industryClassification in industryClassifications)
            {

                Console.WriteLine(industryClassification);

                industryClassificationList.Add(
                    new IndustryClassification
                    {
                        IndustryClassificationId = Guid.NewGuid(),
                        Industry = industryClassification.industry,
                        Sector = industryClassification.sector

                    }
                );
            }

            return industryClassificationList;
        }

        public async Task SaveIndustryClassificationToDatabase(List<IndustryClassification> industryClassifications)
        {
            try
            {
                await _referenceRepository.AddIndustryClassificationsToDatabase(industryClassifications);
                Console.WriteLine("Industry Classifications added to database");
            }
            catch (System.Exception ex)
            {

                throw new Exception("Error saving industry classifications to database, error: " + ex.Message);
            }
        }

    }
}