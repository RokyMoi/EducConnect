using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAllFoldersResponse
    {
        public Guid FolderId { get; set; }
        public string Name { get; set; }
        public Guid? ParentFolderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}