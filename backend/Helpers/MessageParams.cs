namespace EduConnect.Helpers
{
    public class MessageParams:PersonPaginationParams
    {
        //Checking by email that is optional
        public  string? Email {  get; set; }
        //Default value will be "Unread" for request.
        public string Container { get; set; } = "Unread";
    }
}
