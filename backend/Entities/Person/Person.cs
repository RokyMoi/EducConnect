using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Person
{

    [Table("Person", Schema = "Person")]
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }

        //Set to false by default 
        public required bool IsActive { get; set; } = false;

        public required long CreatedAt { get; set; }

        public long? ModifiedAt { get; set; }

        ///Pristupi

        public PersonDetails PersonDetails { get; set; }
        public PersonEmail PersonEmail { get; set; }
    }
}

