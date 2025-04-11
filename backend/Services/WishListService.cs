using EduConnect.Data;
using EduConnect.Entities.Course;
using EduConnect.Entities.Student;
using EduConnect.Interfaces.Shopping;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EduConnect.Entities.Shopping;

namespace EduConnect.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly DataContext _context;
        private readonly IShoppingCartService _shoppingCartService;

        public WishlistService(DataContext context, IShoppingCartService shoppingCartService)
        {
            _context = context;
            _shoppingCartService = shoppingCartService;
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

        public async Task<Wishlist> CreateWishlistAsync(string email)
        {
            var student = await GetStudentByEmailAsync(email);

            var existingWishlist = await _context.WishList
                .FirstOrDefaultAsync(w => w.StudentID == student.StudentId);

            if (existingWishlist != null)
            {
                return existingWishlist;
            }

            var newWishlist = new Wishlist
            {
                WishlistID = Guid.NewGuid(),
                StudentID = student.StudentId,
                Student = student,
                Items = new List<Course>()
            };

            _context.WishList.Add(newWishlist);
            await _context.SaveChangesAsync();

            return newWishlist;
        }

        public async Task<bool> AddCourseToWishlistAsync(string email, Guid courseId)
        {
            var student = await GetStudentByEmailAsync(email);

            var wishlist = await _context.WishList
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.StudentID == student.StudentId)
                ?? throw new InvalidOperationException("Wishlist not found for student");

            var course = await _context.Course.FindAsync(courseId)
                ?? throw new ArgumentException("Course not found");

            if (!wishlist.Items.Contains(course))
            {
                wishlist.Items.Add(course);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveCourseFromWishlistAsync(string email, Guid courseId)
        {
            var student = await GetStudentByEmailAsync(email);

            var wishlist = await _context.WishList
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.StudentID == student.StudentId);

            if (wishlist == null)
            {
                return false;
            }

            var course = await _context.Course.FindAsync(courseId);
            if (course == null)
            {
                return false;
            }

            var removed = wishlist.Items.Remove(course);
            if (removed)
            {
                await _context.SaveChangesAsync();
            }

            return removed;
        }

        public async Task<bool> MoveCourseToShoppingCartAsync(string email, Guid courseId)
        {
            var student = await GetStudentByEmailAsync(email);

            var wishlist = await _context.WishList
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.StudentID == student.StudentId)
                ?? throw new InvalidOperationException("Wishlist not found for student");

            var course = await _context.Course.FindAsync(courseId)
                ?? throw new ArgumentException("Course not found");

            // Remove from wishlist
            if (!wishlist.Items.Remove(course))
            {
                return false;
            }

            // Add to shopping cart
            bool addedToCart = await _shoppingCartService.AddCourseToShoppingCartAsync(email, courseId);

            if (addedToCart)
            {
                await _context.SaveChangesAsync();
                return true;
            }

            // If adding to cart fails, add back to wishlist
            wishlist.Items.Add(course);
            await _context.SaveChangesAsync();

            return false;
        }

        public async Task<Wishlist?> GetWishlistForStudentAsync(string email)
        {
            var student = await GetStudentByEmailAsync(email);

            return await _context.WishList
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.StudentID == student.StudentId);
        }
    }


}