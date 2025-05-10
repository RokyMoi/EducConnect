using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EduConnect.Data;
using EduConnect.Entities.Student;
using EduConnect.Interfaces.Shopping;
using EduConnect.Entities.Course;

namespace EduConnect.Services
{
    /// <summary>
    /// Service for managing student enrollments in courses
    /// </summary>
    public class StudentEnrollmentService : IStudentEnrollmentService
    {
        private readonly DataContext _context;
        private readonly ILogger<StudentEnrollmentService> _logger;

        /// <summary>
        /// Initializes a new instance of the StudentEnrollmentService
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        public StudentEnrollmentService(DataContext context, ILogger<StudentEnrollmentService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves a student by their email address
        /// </summary>
        /// <param name="email">Email address of the student</param>
        /// <returns>Student entity</returns>
        /// <exception cref="ArgumentException">Thrown when email is invalid or student not found</exception>
        private async Task<Student> GetStudentByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogError("Student email is null or empty");
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            _logger.LogInformation("Retrieving student for email: {Email}", email);

            var person = await _context.PersonEmail
                .FirstOrDefaultAsync(x => x.Email == email);

            if (person == null)
            {
                _logger.LogWarning("Person with email {Email} not found", email);
                throw new ArgumentException("Cannot read email from token sent");
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(x => x.PersonId == person.PersonId);

            if (student == null)
            {
                _logger.LogWarning("Student not found for person with ID {PersonId}", person.PersonId);
                throw new ArgumentException("Student not found");
            }

            _logger.LogInformation("Successfully retrieved student with ID {StudentId}", student.StudentId);
            return student;
        }

        /// <summary>
        /// Processes course enrollments for a student based on their shopping cart
        /// </summary>
        /// <param name="studentEmail">Email of the student</param>
        /// <param name="cartId">ID of the shopping cart</param>
        /// <returns>Task representing the asynchronous operation</returns>
        /// <exception cref="ArgumentException">Thrown when student email is invalid, cart not found, or enrollment fails</exception>
        public async Task ProcessCourseEnrollmentFromCartAsync(string studentEmail, Guid cartId)
        {
            try
            {
                _logger.LogInformation("Processing enrollments from cart {CartId} for student {Email}", cartId, studentEmail);

                var findedStudent = await GetStudentByEmailAsync(studentEmail);

                var cart = await _context.ShoppingCart
                    .Include(sc => sc.Items)
                    .FirstOrDefaultAsync(sc => sc.ShoppingCartID == cartId);

                if (cart == null)
                {
                    _logger.LogWarning("Shopping cart with ID {CartId} not found", cartId);
                    throw new ArgumentException("Shopping cart not found");
                }

                if (cart.Items == null || cart.Items.Count == 0)
                {
                    _logger.LogWarning("Shopping cart {CartId} is empty, no enrollments to process", cartId);
                    return;
                }

                _logger.LogInformation("Found {Count} courses in cart {CartId}", cart.Items.Count, cartId);

                foreach (var item in cart.Items)
                {
                    // Check if the student is already enrolled in this course
                    var existingEnrollment = await _context.StudentEnrollment
                        .FirstOrDefaultAsync(e =>
                            e.StudentId == findedStudent.StudentId &&
                            e.CourseId == item.CourseID);

                    if (existingEnrollment != null)
                    {
                        _logger.LogInformation("Student {StudentId} already enrolled in course {CourseId}, skipping",
                            findedStudent.StudentId, item.CourseID);
                        continue;
                    }

                    var enrollment = new StudentEnrollment
                    {
                        StudentEnrollmentId = Guid.NewGuid(),
                        StudentId = findedStudent.StudentId,
                        CourseId = item.CourseID,
                        EnrollmentDate = DateTime.UtcNow,
                        Status = EnrollmentStatus.Active
                    };

                    _context.StudentEnrollment.Add(enrollment);
                    _logger.LogInformation("Created enrollment for student {StudentId} in course {CourseId}",
                        findedStudent.StudentId, item.CourseID);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully completed enrollment process for student {Email}", studentEmail);
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                _logger.LogError(ex, "Error processing enrollments from cart {CartId} for student {Email}",
                    cartId, studentEmail);
                throw;
            }
        }
    }
}