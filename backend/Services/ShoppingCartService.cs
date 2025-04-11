using EduConnect.Data;
using EduConnect.Entities.Course;
using EduConnect.Entities.Shopping;
using EduConnect.Entities.Student;
using EduConnect.Interfaces.Shopping;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly DataContext _context;
        private readonly IWishlistService _wishlistService;

        public ShoppingCartService(
            DataContext context,
            IWishlistService wishlistService)
        {
            _context = context;
            _wishlistService = wishlistService;
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

        public async Task<ShoppingCart> CreateShoppingCartAsync(string email)
        {
            var student = await GetStudentByEmailAsync(email);

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
            var student = await GetStudentByEmailAsync(email);

            var shoppingCart = await _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(x => x.StudentID == student.StudentId);

            if (shoppingCart == null)
            {
                return false;
            }

            var course = await _context.Course.FindAsync(courseID);
            if (course == null)
            {
                return false;
            }

            var removed = shoppingCart.Items.Remove(course);
            if (removed)
            {
                await _context.SaveChangesAsync();
            }

            return removed;
        }

        public async Task<ShoppingCart?> GetShoppingCartForStudentAsync(string email)
        {
            var student = await GetStudentByEmailAsync(email);

            return await _context.ShoppingCart
                .Include(sc => sc.Items)
                    .ThenInclude(c => c.CourseDetails)
                .FirstOrDefaultAsync(x => x.StudentID == student.StudentId);
        }

        public async Task<decimal> GetTotalPriceAsync(Guid shoppingCartId)
        {
            var shoppingCart = await _context.ShoppingCart
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.ShoppingCartID == shoppingCartId)
                ?? throw new ArgumentException("There is no shopping cart with that id");

            return (decimal)await _context.CourseDetails
                .Where(cd => shoppingCart.Items.Select(i => i.CourseId).Contains(cd.CourseId))
                .SumAsync(cd => cd.Price);
        }

        public async Task<bool> AddCourseToShoppingCartAsync(string email, Guid courseID)
        {
            var student = await GetStudentByEmailAsync(email);

            var shoppingCart = await _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(x => x.StudentID == student.StudentId)
                ?? throw new InvalidOperationException("Shopping cart not found for student");

            var course = await _context.Course
                .Include(c => c.CourseDetails)
                .FirstOrDefaultAsync(c => c.CourseId == courseID)
                ?? throw new ArgumentException("Course not found");

            if (!shoppingCart.Items.Contains(course))
            {
                shoppingCart.Items.Add(course);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> MoveCourseToWishListAsync(string email, Guid courseId)
        {
            var student = await GetStudentByEmailAsync(email);

            var shoppingCart = await _context.ShoppingCart
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.StudentID == student.StudentId)
                ?? throw new InvalidOperationException("Shopping cart not found for student");

            var course = await _context.Course.FindAsync(courseId)
                ?? throw new ArgumentException("Course not found");

            // Remove from shoppingCart
            if (!shoppingCart.Items.Remove(course))
            {
                return false;
            }

            // Add to wishlist 
            bool addedToWishList = await _wishlistService.AddCourseToWishlistAsync(email, courseId);

            if (addedToWishList)
            {
                await _context.SaveChangesAsync();
                return true;
            }

            // If adding to wishlist fails, add back to cart
            shoppingCart.Items.Add(course);
            await _context.SaveChangesAsync();

            return false;
        }

        // Additional helper methods
        public async Task<bool> ClearShoppingCartAsync(string email)
        {
            var student = await GetStudentByEmailAsync(email);

            var shoppingCart = await _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(x => x.StudentID == student.StudentId);

            if (shoppingCart == null)
            {
                return false;
            }

            shoppingCart.Items.Clear();
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<ShoppingCart?> GetShoppingCartByIdAsync(Guid cartId)
        {
            return await _context.ShoppingCart
                .Include(sc => sc.Items)
                    .ThenInclude(c => c.CourseDetails)
                .FirstOrDefaultAsync(sc => sc.ShoppingCartID == cartId);
        }
        public async Task<int> GetItemCountAsync(string email)
        {
            var student = await GetStudentByEmailAsync(email);

            var shoppingCart = await _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(x => x.StudentID == student.StudentId);

            return shoppingCart?.Items.Count ?? 0;
        }
    }
}