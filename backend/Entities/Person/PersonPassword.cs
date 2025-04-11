using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Entities.Person;

[Table("PersonPassword", Schema = "Person")]
public class PersonPassword
{
    [Key]
    public Guid PersonPasswordId { get; set; }

    public Guid PersonId { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person? Person { get; set; }


    public string PasswordHash { get; set; } 

    public long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; }
}

