using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Tutor
{
    public class TutorTeachingInformationSaveRequestDTO
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public int TeachingStyleTypeId { get; set; }

        [Required]
        public int PrimaryCommunicationTypeId { get; set; }

        public int? SecondaryCommunicationTypeId { get; set; }

        [Required]
        public int PrimaryEngagementMethodId { get; set; }

        public int? SecondaryEngagementMethodId { get; set; }

        public int ExpectedResponseTime { get; set; }

        public string? SpecialConsiderations { get; set; }

    }
}