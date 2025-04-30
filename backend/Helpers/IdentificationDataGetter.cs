using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Person;
using EduConnect.Entities.Person;

namespace EduConnect.Helpers
{
    public static class IdentificationDataGetter
    {
        public static string GetIdentificationData(PersonEmail personEmail, PersonDetails personDetails)
        {
            if (personEmail == null && personDetails == null)
            {
                return "Unknown";
            }

            var email = personEmail?.Email ?? string.Empty;


            var firstName = personDetails.FirstName ?? string.Empty;
            var lastName = personDetails?.LastName ?? string.Empty;

            var identificationData = string.Empty;

            if (!string.IsNullOrEmpty(firstName))
            {
                identificationData += firstName;
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                identificationData += " " + lastName;
            }

            if (!string.IsNullOrEmpty(email) && string.IsNullOrEmpty(identificationData))
            {
                identificationData += email;
            }
            else if (!string.IsNullOrEmpty(email))
            {
                identificationData += " (" + email + ")";
            }

            if (string.IsNullOrEmpty(identificationData))
            {
                return "Unknown";
            }

            return identificationData.Trim();
        }
    }
}