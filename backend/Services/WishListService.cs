using EduConnect.Data;
using EduConnect.Entities.Course;
using EduConnect.Entities.Shopping;
using EduConnect.Entities.Student;
using EduConnect.Interfaces.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Services
{
    /// <summary>
    /// Service for managing wishlist operations
    /// </summary>
    public class WishlistService : IWishlistService
    {
        private readonly DataContext _context;
        private readonly ILogger<WishlistService> _logger;

        public WishlistService(DataContext context, ILogger<WishlistService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task<Student> GetStudentByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be null or empty", nameof(email));

            var person = await _context.PersonEmail.FirstOrDefaultAsync(x => x.Email == email);
            if (person == null)
                throw new ArgumentException("Cannot read email from token sent", nameof(email));

            var student = await _context.Student.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
            if (student == null)
                throw new ArgumentException("Student not found", nameof(email));

            return student;
        }

        private async Task<Wishlist> GetOrCreateWishlistAsync(string email)
        {
            var student = await GetStudentByEmailAsync(email);

            var wishlist = await _context.Wishlist
                .Include(w => w.Items)
                .ThenInclude(i => i.Course)
                .ThenInclude(c => c.CourseDetails)
                .FirstOrDefaultAsync(w => w.StudentID == student.StudentId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    WishlistID = Guid.NewGuid(),
                    StudentID = student.StudentId,
                    Student = student
                };

                _context.Wishlist.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            return wishlist;
        }

        public async Task<Wishlist?> GetWishlistForStudentAsync(string email)
        {
            try
            {
                var student = await GetStudentByEmailAsync(email);

                return await _context.Wishlist
                    .Include(w => w.Items)
                    .ThenInclude(i => i.Course)
                    .ThenInclude(c => c.CourseDetails)
                    .FirstOrDefaultAsync(w => w.StudentID == student.StudentId);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Failed to get wishlist for {Email}", email);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wishlist for {Email}", email);
                throw;
            }
        }

        public async Task<bool> AddCourseToWishlistAsync(string email, Guid courseId)
        {
            try
            {
                var wishlist = await GetOrCreateWishlistAsync(email);

                // Proveri da li je kurs već u listi želja
                if (wishlist.Items.Any(item => item.CourseID == courseId))
                {
                    return false; // Već postoji u listi
                }

                // Proveri da li kurs postoji
                var course = await _context.Course
                    .Include(c => c.CourseDetails)
                    .FirstOrDefaultAsync(c => c.CourseId == courseId);

                if (course == null)
                {
                    throw new ArgumentException("Course not found", nameof(courseId));
                }

                // Kreiraj novi WishlistItem
                var wishlistItem = new WishlistItems
                {
                    WishtListItemId = Guid.NewGuid(),
                    WishListId = wishlist.WishlistID,
                    CourseID = courseId,
                    Course = course,
                    AddedAt = DateTime.UtcNow
                };

                // Direktno dodaj novi red u kontekst (ne preko kolekcije)
                _context.WishlistItems.Add(wishlistItem);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                _logger.LogError(ex, "Error adding course {CourseId} to wishlist for {Email}", courseId, email);
                throw;
            }
        }

        public async Task<bool> RemoveCourseFromWishlistAsync(string email, Guid courseId)
        {
            try
            {
                var wishlist = await GetOrCreateWishlistAsync(email);

                var item = wishlist.Items.FirstOrDefault(i => i.CourseID == courseId);
                if (item != null)
                {
                    wishlist.Items.Remove(item);
                    _context.WishlistItems.Remove(item);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing course {CourseId} from wishlist for {Email}", courseId, email);
                throw;
            }
        }

        public async Task<bool> IsCourseInWishlistAsync(string email, Guid courseId)
        {
            try
            {
                var wishlist = await GetWishlistForStudentAsync(email);
                return wishlist?.Items.Any(i => i.CourseID == courseId) ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking course {CourseId} in wishlist for {Email}", courseId, email);
                throw;
            }
        }

        public async Task<int> GetItemCountAsync(string email)
        {
            try
            {
                var wishlist = await GetWishlistForStudentAsync(email);
                return wishlist?.Items.Count ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting wishlist item count for {Email}", email);
                throw;
            }
        }

        public async Task<bool> ClearWishlistAsync(string email)
        {
            try
            {
                var wishlist = await GetOrCreateWishlistAsync(email);

                var itemsToRemove = wishlist.Items.ToList();
                foreach (var item in itemsToRemove)
                {
                    _context.WishlistItems.Remove(item);
                }

                wishlist.Items.Clear();
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing wishlist for {Email}", email);
                throw;
            }
        }
    }
}
