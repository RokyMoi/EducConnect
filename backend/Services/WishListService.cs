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
    public class WishlistService(DataContext _context) : IWishlistService
    {
       
       

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

            // Dohvat wishlist-a studenta
            var wishlist = await _context.WishList
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.StudentID == student.StudentId)
                ?? throw new InvalidOperationException("Wishlist not found for student");

            // Dohvat kursa na osnovu courseId-a
            var course = await _context.Course.FindAsync(courseId)
                ?? throw new ArgumentException("Course not found");

            // Uklanjanje kursa iz wishlist-a
            if (!wishlist.Items.Remove(course))
            {
                return false; // Ako kurs nije pronađen u wishlist-u
            }

            // Dohvat shopping cart-a studenta
            var shoppingCart = await _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(sc => sc.StudentID == student.StudentId)
                ?? throw new InvalidOperationException("Shopping cart not found for student");

            // Dodavanje kursa u shopping cart ako nije već prisutan
            if (!shoppingCart.Items.Contains(course))
            {
                shoppingCart.Items.Add(course);
                await _context.SaveChangesAsync();
                return true;
            }

            // Ako je kurs već u shopping cart-u, vraćamo ga nazad u wishlist
            wishlist.Items.Add(course);
            await _context.SaveChangesAsync();

            return false; // Kurs je već bio u shopping cart-u
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