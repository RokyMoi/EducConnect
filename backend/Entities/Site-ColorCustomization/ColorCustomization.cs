using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConnect.Entities.Site_ColorCustomization
{
    public class ColorCustomization
    {
        [Key]
        public Guid CustomizationID { get; set; }
        public Guid PersonId { get; set; }

        [ForeignKey(nameof(PersonId))]
        public Person.Person Person { get; set; }
        public string? HexColor { get; set; }
        public string? DarkModeColor { get; set; }
        public bool? IsDarkMode { get; set; }
    }
}
