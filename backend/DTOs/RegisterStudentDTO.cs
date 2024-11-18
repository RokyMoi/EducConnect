namespace EduConnect.DTOs
{
    public class RegisterStudentDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }

        public required string Password { get; set; }
        public required string Username { get; set; }
        public required string PhoneNumberCountryCode { get; set; }

        public required string PhoneNumber { get; set; }

        public required string CountryOfOrigin { get; set; }
    }
}
