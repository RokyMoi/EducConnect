using EduConnect.Entities.Messenger;
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

        ///Navigations

        public PersonDetails PersonDetails { get; set; }
        public ICollection<PersonPhoto> PersonPhoto { get; set; }
        public PersonEmail PersonEmail { get; set; }

        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }
    }
}

