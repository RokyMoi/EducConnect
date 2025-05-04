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

        public ICollection<WishlistItems> Items { get; set; } = new List<WishlistItems>();

        public required Guid StudentID { get; set; }

        [ForeignKey(nameof(StudentID))]
        public Student.Student Student { get; set; }
    }

    public class WishlistItems
    {
        [Key]
        public required Guid WishtListItemId { get; set; }

        public required Guid WishListId { get; set; }

        [ForeignKey(nameof(WishListId))]
        public Wishlist WishList { get; set; }


        public required Guid CourseID { get; set; }

        [ForeignKey(nameof(CourseID))]
        public Course.Course Course { get; set; }

        public DateTime AddedAt { get; set; }

    }
}
