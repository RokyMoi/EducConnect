using EduConnect.Helpers;

namespace EduConnect.DTOs.Messenger
{
    public class MessageParamsDirect : PersonPaginationParams
    {
        //Checking by email that is optional
        public string? Email { get; set; }
        //Default value will be "Unread" for request.
    
    }
}
