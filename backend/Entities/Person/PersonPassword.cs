using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Entities.Person;

public class PersonPassword
{
    [Key]
    public Guid PersonPasswordId { get; set; }

    public Guid PersonId { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person? Person { get; set; }


    public byte[] Hash { get; set; } = []; // Prazan niz

    public byte[] Salt { get; set; } = []; // Prazan niz

    public long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; }
}

