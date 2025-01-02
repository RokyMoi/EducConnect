namespace EduConnect.DTOs.Messenger
{
    public class CreateMessageDto
    {
        public required string RecipientEmail { get; set; }
        public required string Content { get; set; }
    }
}
