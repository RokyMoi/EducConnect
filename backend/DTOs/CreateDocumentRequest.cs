using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class CreateDocumentRequest
    {
        [Length(10, 255)]
        public string Title { get; set; }

    }
}