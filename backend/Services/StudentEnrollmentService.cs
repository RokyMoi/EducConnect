using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EduConnect.Data;
using EduConnect.Entities.Student;
using EduConnect.Interfaces.Shopping;
using EduConnect.Entities.Course;

namespace EduConnect.Services
{
 
    public class StudentEnrollmentService : IStudentEnrollmentService
    {
        private readonly DataContext _context;

        public StudentEnrollmentService(DataContext context)
        {
            _context = context;
        }
        private async Task<Student> GetStudentByEmailAsync(string email)
        {
            var person = await _context.PersonEmail
                .FirstOrDefaultAsync(x => x.Email == email)
                ?? throw new ArgumentException("Cannot read email from token sent");

            return await _context.Student
                .FirstOrDefaultAsync(x => x.PersonId == person.PersonId)
                ?? throw new ArgumentException("Student not found");
        }

        public async Task ProcessCourseEnrollmentFromCartAsync(string studentEmail, Guid cartId)
        { 
            var findedStudent = await GetStudentByEmailAsync(studentEmail);

          
            var cart = await _context.ShoppingCart
                .Include(sc => sc.Items)
                    .ThenInclude(c => c.CourseDetails)
                .FirstOrDefaultAsync(sc => sc.ShoppingCartID == cartId);

            if (cart == null)
            {
                throw new ArgumentException("Shopping cart not found");
            }

       
            foreach (var course in cart.Items)
            {
                var enrollment = new StudentEnrollment
                {
                    StudentEnrollmentId = Guid.NewGuid(),
                    StudentId =findedStudent.StudentId,
                    CourseId = course.CourseId,
                    EnrollmentDate = DateTime.UtcNow,
                    Status = EnrollmentStatus.Active
                };

                _context.StudentEnrollment.Add(enrollment);
            }

            await _context.SaveChangesAsync();
        }
    }
}