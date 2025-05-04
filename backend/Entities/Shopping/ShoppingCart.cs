using EduConnect.Entities.Course;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Shopping
{
    public class ShoppingCart
    {
        [Key]
        public required Guid ShoppingCartID { get; set; }

        public ICollection<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public required Guid StudentID { get; set; }

        [ForeignKey(nameof(StudentID))]
        public Student.Student Student { get; set; }

        public DateTime LastModified { get; internal set; }
        public DateTime CreatedAt { get; internal set; }
    }

    public class ShoppingCartItem
    {
        [Key]
        public required Guid ShoppingCartItemID { get; set; }

        public required Guid ShoppingCartID { get; set; }

        [ForeignKey(nameof(ShoppingCartID))]
        public ShoppingCart ShoppingCart { get; set; }

       
        public required Guid CourseID { get; set; }

        [ForeignKey(nameof(CourseID))]
        public Course.Course Course { get; set; }

        public DateTime AddedAt { get; set; }
       
    }
}