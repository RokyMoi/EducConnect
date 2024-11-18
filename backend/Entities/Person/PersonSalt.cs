using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Entities.Person;

public class PersonSalt
{
    [Key]
    public Guid PersonSaltId { get; set; }

    public Guid PersonId { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person? Person { get; set; }


    public string Salt { get; set; }  // Prazan niz

    public int NumberOfRounds { get; set; }

    public long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; }
}

