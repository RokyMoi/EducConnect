using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;



namespace EduConnect.Entities.Tutor;

[Table("Tutor", Schema = "Tutor")]
public class Tutor
{

    [Key]
    public Guid TutorId { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person.Person Person { get; set; }
    public Guid PersonId { get; set; }

    public int TutorRegistrationStatusId { get; set; }
    [ForeignKey(nameof(TutorRegistrationStatusId))]
    public TutorRegistrationStatus TutorRegistrationStatus { get; set; }

    public long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; }



}

