using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Tutor
{
    public class TutorTeachingInformationUpdateRequestDTO
    {
        public string? Description { get; set; }
        [Required]
        public bool UpdateDescription { get; set; }

        public int? TeachingStyleTypeId { get; set; }


        public int? PrimaryCommunicationTypeId { get; set; }

        public int? SecondaryCommunicationTypeId { get; set; }
        [Required]
        public bool UpdateSecondaryCommunicationTypeId { get; set; }

        public int? PrimaryEngagementMethodId { get; set; }

        public int? SecondaryEngagementMethodId { get; set; }
        
        [Required]
        public bool UpdateSecondaryEngagementMethodId { get; set; }

        public int? ExpectedResponseTime { get; set; }
        [Required]
        public bool UpdateExpectedResponseTime { get; set; }
        public string? SpecialConsiderations { get; set; }
        [Required]
        public bool UpdateSpecialConsiderations { get; set; }
    }
}