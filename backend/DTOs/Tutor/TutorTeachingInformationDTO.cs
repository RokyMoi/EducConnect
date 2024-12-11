using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Tutor
{
    public class TutorTeachingInformationDTO
    {

        public Guid TutorId { get; set; }
        public string? Description { get; set; }


        public int TeachingStyleTypeId { get; set; }


        public int PrimaryCommunicationTypeId { get; set; }

        public int? SecondaryCommunicationTypeId { get; set; }


        public int PrimaryEngagementMethodId { get; set; }

        public int? SecondaryEngagementMethodId { get; set; }

        public int? ExpectedResponseTime { get; set; }

        public string? SpecialConsiderations { get; set; }
    }
}