using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EduConnect.Entities.Person
{

    [Table("Person", Schema = "Person")]
    public class Person : IdentityUser<Guid>
    {
        [Key]
        public Guid PersonId { get; set; }

        //Set to false by default 
        public required bool IsActive { get; set; } = false;

        public Guid PersonPublicId { get; set; } = Guid.NewGuid();
        public required long CreatedAt { get; set; }

        public long? ModifiedAt { get; set; }

        ///Pristupi

        public PersonDetails PersonDetails { get; set; }
        public PersonEmail PersonEmail { get; set; }
        public PersonSalt PersonSalt { get; set; }

        public override Guid Id
        {
            get => PersonId;
            set => PersonId = value;
        }
    }
}

