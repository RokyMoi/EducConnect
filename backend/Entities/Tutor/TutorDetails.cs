using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Entities.Tutor;

[Table("TutorDetails", Schema = "Tutor")]
public class TutorDetails
{

    [Key]
    public Guid TutorDetailsId { get; set; }

    public Guid TutorId { get; set; }

    [ForeignKey(nameof(TutorId))]
    public Tutor Tutor { get; set; }

    public string? ShortBiography { get; set; }

    public required int YearsOfExperience { get; set; }

    public required string MainAreaOfSpecialization { get; set; }

    public long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; }

}
