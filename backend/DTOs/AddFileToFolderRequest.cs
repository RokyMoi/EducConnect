using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class AddFileToFolderRequest
    {
        public Guid FolderId { get; set; }
        public Guid File { get; set; }
    }
}