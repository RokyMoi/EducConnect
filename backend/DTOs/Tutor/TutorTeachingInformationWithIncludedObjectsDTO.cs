using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities.Reference;

namespace backend.DTOs.Tutor
{
    public class TutorTeachingInformationWithIncludedObjectsDTO
    {
        public Guid TutorTeachingInformationId { get; set; }

        public Guid TutorId { get; set; }

        public string? Description { get; set; }

        public TutorTeachingStyleType TutorTeachingStyleType { get; set; }

        public CommunicationType PrimaryCommunicationType { get; set; }

        public CommunicationType? SecondaryCommunicationType { get; set; }

        public EngagementMethod PrimaryEngagementMethod { get; set; }

        public EngagementMethod? SecondaryEngagementMethod { get; set; }

        public string? ExpectedResponseTime { get; set; }

        public string? SpecialConsiderations { get; set; }


    }
}