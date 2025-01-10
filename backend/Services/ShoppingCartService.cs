using EduConnect.Data;
using EduConnect.Entities.Course;
using EduConnect.Entities.Shopping;
using EduConnect.Helpers;
using EduConnect.Interfaces.Shopping;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly DataContext _context;

        public ShoppingCartService(DataContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart> CreateShoppingCartAsync(string email)
        {
            var person = await _context.PersonEmail.FirstOrDefaultAsync(x => x.Email == email);
            if (person == null)
            {
                throw new ArgumentException("Cannot read email from token sent");
            }

            var student = await _context.Student.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
            if (student == null)
            {
                throw new ArgumentException("Student not found");
            }

            var existingCart = await _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(sc => sc.StudentID == student.StudentId);

            if (existingCart != null)
            {
                return existingCart;
            }

            var newCart = new ShoppingCart
            {
                ShoppingCartID = Guid.NewGuid(),
                StudentID = student.StudentId,
                Student = student,
                Items = new List<Course>()
            };

            _context.ShoppingCart.Add(newCart);
            await _context.SaveChangesAsync();

            return newCart;
        }

        public async Task<bool> DeleteShoppingCartItemAsync(string email, Guid courseID)
        {
            var person = await _context.PersonEmail.FirstOrDefaultAsync(x => x.Email == email);
            if (person == null)
            {
                throw new ArgumentException("Cannot read email from token sent");
            }

            var student = await _context.Student.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
            if (student == null)
            {
                throw new ArgumentException("Student not found");
            }

            var shoppingCart = await _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(x => x.StudentID == student.StudentId);

            if (shoppingCart == null)
            {
                return false;
            }

            var course = await _context.Course.FirstOrDefaultAsync(x => x.CourseId == courseID);
            if (course == null || !shoppingCart.Items.Remove(course))
            {
                return false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ShoppingCart?> GetShoppingCartForStudentAsync(string email)
        {
            var person = await _context.PersonEmail.FirstOrDefaultAsync(x => x.Email == email);
            if (person == null)
            {
                throw new ArgumentException("Cannot read email from token sent");
            }

            var student = await _context.Student.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
            if (student == null)
            {
                throw new ArgumentException("Student not found");
            }

            return await _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(x => x.StudentID == student.StudentId);
        }

        public async Task<long> GetTotalPriceAsync(Guid shoppingCartId)
        {
         
            var shoppingCart = await _context.ShoppingCart
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.ShoppingCartID == shoppingCartId);

            if (shoppingCart == null)
            {
                throw new ArgumentException("There is no shopping cart with that id");
            }

            var totalAmount = await _context.CourseDetails
                .Where(cd => shoppingCart.Items.Select(i => i.CourseId).Contains(cd.CourseId))
                .SumAsync(cd => (long)cd.Price);

            return totalAmount;
        }

        public async Task<bool> SetShoppingCartAsync(string email, Guid courseID)
        {
            var person = await _context.PersonEmail.FirstOrDefaultAsync(x => x.Email == email);
            if (person == null)
            {
                throw new ArgumentException("Cannot read email from token sent");
            }

            var student = await _context.Student.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
            if (student == null)
            {
                throw new ArgumentException("Student not found");
            }

            var shoppingCart = await _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(x => x.StudentID == student.StudentId);

            if (shoppingCart == null)
            {
                return false;
            }

            var course = await _context.Course.FirstOrDefaultAsync(x => x.CourseId == courseID);
            if (course == null)
            {
                throw new ArgumentException("Course not found");
            }

            if (!shoppingCart.Items.Contains(course))
            {
                shoppingCart.Items.Add(course);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}
