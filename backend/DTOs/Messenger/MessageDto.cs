using EduConnect.Entities.Person;

namespace EduConnect.DTOs.Messenger
{
    public class MessageDto
    {
        public int Id { get; set; }
        public Guid SenderId { get; set; }
        public required string SenderEmail { get; set; }
        public  required string SenderPhotoUrl { get; set; }
        public Guid RecipientId { get; set; }
        public required string RecipientPhotoUrl { get; set; }
        public required string RecipientEmail { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
  

    }
}
