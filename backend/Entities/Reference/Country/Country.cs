using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Org.BouncyCastle.Bcpg;

namespace backend.Entities.Reference.Country
{
    [Table("Country", Schema = "Reference")]
    public class Country
    {
        public Guid CountryId { get; set; }
        public string OfficialName { get; set; }
        public string CommonName { get; set; }

        public string NationalCallingCode { get; set; }
        public string ISOAlpha2Code { get; set; }

        public string FlagUrl { get; set; }
        public string FlagEmoji { get; set; }
        

    }
}