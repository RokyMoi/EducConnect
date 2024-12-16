using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EduConnect.Entities.Person
{
    public class PersonPhoto
    {

        [Key]
        public int Id { get; set; }
        public required string Url { get; set; }

        public string? PublicId { get; set; }

        public Guid PersonId { get; set; }
        [ForeignKey(nameof(PersonId))]
        public Person Person { get; set; }




    }
}
