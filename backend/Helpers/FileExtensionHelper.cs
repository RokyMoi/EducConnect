using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Helpers
{
    public static class FileExtensionHelper
    {
        public static string GetFileExtension(string contentType)
        {
            return contentType.Split("/")[1];
        }
    }
}