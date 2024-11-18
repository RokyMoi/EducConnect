using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Entities.Tutor;

public class TutorAvailability
{
    [Key]
    public Guid TutorAvailabilityId { get; set; }

    public Guid TutorId { get; set; }

    [ForeignKey(nameof(TutorId))]
    public required Tutor Tutor { get; set; }

    public required string DayOfWeek { get; set; }

    public required string StartTime { get; set; }

    public required string EndTime { get; set; }

    public long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; }
}
