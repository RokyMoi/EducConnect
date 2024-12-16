using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Entities.Tutor
{
    [Table("TutorTeachingInformation", Schema = "Tutor")]
    [Index(nameof(TutorId), IsUnique = true)]
    public class TutorTeachingInformation
    {
        [Key]
        public Guid TutorTeachingInformationId { get; set; }

        
        public Guid TutorId { get; set; }

        [ForeignKey("TutorId")]
        public EduConnect.Entities.Tutor.Tutor Tutor { get; set; }
        public string? Description { get; set; }
        public int TeachingStyleTypeId { get; set; }

        [ForeignKey("TeachingStyleTypeId")]
        public EduConnect.Entities.Reference.TutorTeachingStyleType TeachingStyleType { get; set; }

        public int PrimaryCommunicationTypeId { get; set; }

        [ForeignKey("PrimaryCommunicationTypeId")]
        public EduConnect.Entities.Reference.CommunicationType PrimaryCommunicationType { get; set; }

        public int? SecondaryCommunicationTypeId { get; set; }
        [ForeignKey("SecondaryCommunicationTypeId")]
        public EduConnect.Entities.Reference.CommunicationType? SecondaryCommunicationType { get; set; }

        public int PrimaryEngagementMethodId { get; set; }

        [ForeignKey("PrimaryEngagementMethodId")]
        public EduConnect.Entities.Reference.EngagementMethod PrimaryEngagementMethod { get; set; }

        public int? SecondaryEngagementMethodId { get; set; }

        [ForeignKey("SecondaryEngagementMethodId")]
        public EduConnect.Entities.Reference.EngagementMethod SecondaryEngagementMethod { get; set; }

        public int? ExpectedResponseTime { get; set; }

        public string? SpecialConsiderations { get; set; }

        public long CreatedAt { get; set; }

        public long? UpdatedAt { get; set; } = null;
    }
}