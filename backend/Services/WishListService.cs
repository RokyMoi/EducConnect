using EduConnect.Data;
using EduConnect.Entities.Course;
using EduConnect.Entities.Shopping;
using EduConnect.Interfaces.Shopping;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services
{
    public class WishListService:IWishListCourse
    {
        private readonly DataContext _context;

        public WishListService(DataContext context)
        {
            _context = context;
        }

        public async Task<WishList> CreateWishListCartAsync(string email)
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

            var existingWishList = await _context.WishList
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(sc => sc.StudentID == student.StudentId);

            if (existingWishList != null)
            {
                return existingWishList;
            }

            var newCart = new WishList
            {
                WishListId = Guid.NewGuid(),
                StudentID = student.StudentId,
                Student = student,
                Items = new List<Course>()
            };

            _context.WishList.Add(newCart);
            await _context.SaveChangesAsync();

            return newCart;
        }

        public async Task<bool> DeleteWishlistItemAsync(string email, Guid courseID)
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

            var wishlist = await _context.WishList
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(x => x.StudentID == student.StudentId);

            if (wishlist == null)
            {
                return false;
            }

            var course = await _context.Course.FirstOrDefaultAsync(x => x.CourseId == courseID);
            if (course == null || !wishlist.Items.Remove(course))
            {
                return false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

       public async Task<WishList?> GetWishListForStudentAsync(string email)
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

            return await _context.WishList
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(x => x.StudentID == student.StudentId);
        }

        public async Task<bool> SetWishlistAsync(string email, Guid courseID)
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

            var wishlist = await _context.WishList
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(x => x.StudentID == student.StudentId);

            if (wishlist == null)
            {
                return false;
            }

            var course = await _context.Course.FirstOrDefaultAsync(x => x.CourseId == courseID);
            if (course == null)
            {
                throw new ArgumentException("Course not found");
            }

            if (!wishlist.Items.Contains(course))
            {
                wishlist.Items.Add(course);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    
}
}
