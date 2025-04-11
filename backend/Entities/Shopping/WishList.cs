using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EduConnect.Entities.Course;
using EduConnect.Entities.Student;

namespace EduConnect.Entities.Shopping
{
    public class Wishlist
    {
        [Key]
        public required Guid WishlistID { get; set; }

        public ICollection<Course.Course> Items { get; set; } = new List<Course.Course>();

        public required Guid StudentID { get; set; }

        [ForeignKey(nameof(StudentID))]
        public Student.Student Student { get; set; }
    }
}
