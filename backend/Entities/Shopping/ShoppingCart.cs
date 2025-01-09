

using EduConnect.Entities.Course;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Shopping
{
    public class ShoppingCart
    {
        [Key]
        public required Guid ShoppingCartID { get; set; }

       
        public ICollection<Course.Course> Items { get; set; } = new List<Course.Course>();

        public required Guid StudentID { get; set; }

        [ForeignKey(nameof(StudentID))]
        public Student.Student Student { get; set; }
    }
}
