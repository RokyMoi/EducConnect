using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Entities.Person;

public class PersonProfilePicture
{

    [Key]
    public Guid PersonProfilePictureId { get; set; }

    public Guid PersonId { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person Person { get; set; }

    public required string Url { get; set; }
    public string? PublicId { get; set; }
    public long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; }

}

