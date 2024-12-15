using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using backend.Entities.Reference.Country;

namespace EduConnect.Entities.Person;

[Table("PersonDetails", Schema = "Person")]
public class PersonDetails
{
    [Key]
    public Guid PersonDetailsId { get; set; }

    public Guid PersonId { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person? Person { get; set; }  // Ovaj entitet može biti null

    public string? FirstName { get; set; } = string.Empty;

    public string? LastName { get; set; } = string.Empty;

    public required string Username { get; set; }

    public Guid? CountryOfOriginCountryId { get; set; }

    [ForeignKey("CountryOfOriginCountryId")]
    public Country? Country { get; set; }

    public required long CreatedAt { get; set; }

    public long? ModifiedAt { get; set; } = null;
}

