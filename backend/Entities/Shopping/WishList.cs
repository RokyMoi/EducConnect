using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduConnect.Entities.Shopping
{
    public class WishList
    {
        [Key]
        public required Guid WishListId { get; set; }


        public ICollection<Course.Course> Items { get; set; } = new List<Course.Course>();

        public required Guid StudentID { get; set; }

        [ForeignKey(nameof(StudentID))]
        public Student.Student Student { get; set; }
    }
}
