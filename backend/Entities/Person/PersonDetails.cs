using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Entities.Person;

[Table("PersonDetails", Schema = "Person")]
public class PersonDetails
{
    [Key]
    public Guid PersonDetailsId { get; set; }

    public Guid PersonId { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person? Person { get; set; }  // Ovaj entitet može biti null

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Username { get; set; }

    public required string PhoneNumberCountryCode { get; set; }

    public required string PhoneNumber { get; set; }

    public required string CountryOfOrigin { get; set; }

    public required long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; }
}

