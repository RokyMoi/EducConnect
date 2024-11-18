using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Entities.Person;

public class PersonEmail
{

    [Key]
    public Guid PersonEmailId { get; set; }

    public Guid PersonId { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person Person { get; set; }

    public required string Email { get; set; }

    public required long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; }

}

