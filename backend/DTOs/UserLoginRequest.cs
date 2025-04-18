using System.ComponentModel.DataAnnotations;

namespace EduConnect.DTOs
{
    public class UserLoginRequest
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
