

using EduConnect.Entities.Person;

namespace EduConnect.Entities.Messenger

{
    public class Message
    {
        public int Id { get; set; }
        public required string SenderEmail { get; set; }
        public required string RecipientEmail { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
        //Flag for users if deleted some message
        public bool SenderDeleted { get; set; }
       
        public bool RecipientDeleted {  get; set; }

        /// navigation properties

        public  Guid SenderId { get; set; }
        public Person.Person Sender { get; set; }
        public Guid RecipientId { get; set; }
        public Person.Person Recipient { get; set; }

    }
}
