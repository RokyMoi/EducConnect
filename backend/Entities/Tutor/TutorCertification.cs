using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Entities.Tutor;

public class TutorCertification
{
    [Key]
    public Guid TutorCertificationId { get; set; }

    public Guid TutorId { get; set; }

    [ForeignKey(nameof(TutorId))]
    public Tutor Tutor { get; set; }

    public required string CertificateName { get; set; }

    public required string IssuedBy { get; set; }

    public long IssueDate { get; set; }

    public long? ExpirationDate { get; set; }

    public string? LinkToCertificate { get; set; }

    public byte[]? CertificateScan { get; set; }

    public long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; }
}
