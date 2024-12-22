namespace EduConnect.Helpers
{
    public class MessageParams:PersonPaginationParams
    {
        public  string? Email {  get; set; }
        public string Container { get; set; } = "Unread";
    }
}
