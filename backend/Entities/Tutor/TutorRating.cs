using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Entities.Tutor;

public class TutorRating
{

    [Key]
    public Guid TutorRatingId { get; set; }

    public Guid TutorId { get; set; }

    [ForeignKey(nameof(TutorId))]
    public Tutor Tutor { get; set; }

    public Guid StudentId { get; set; }


    [ForeignKey(nameof(StudentId))]
    // public Student Student { get; set; }

    public int RatingScore { get; set; }  // min: 0, max: 10

    public string Comment { get; set; }

    public long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; }

}
