using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class CreateFolderRequest
    {
        public Guid? ParentFolderId { get; set; } = null;

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}