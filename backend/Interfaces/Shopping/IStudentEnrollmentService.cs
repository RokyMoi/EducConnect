namespace EduConnect.Interfaces.Shopping
{
    public interface IStudentEnrollmentService
    {
        Task ProcessCourseEnrollmentFromCartAsync(string studentEmail, Guid cartId);
    }
}
