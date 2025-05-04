using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Course
{
    [Table("Folder", Schema = "Course")]
    public class Folder
    {
        [Key]
        public Guid FolderId { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        public Guid? ParentFolderId { get; set; }

        [ForeignKey(nameof(ParentFolderId))]
        public Folder? ParentFolder { get; set; } = null;

        public Guid OwnerPersonId { get; set; }

        [ForeignKey(nameof(OwnerPersonId))]
        public Person.Person? OwnerPerson { get; set; } = null;

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public long? UpdatedAt { get; set; } = null;
    }
}