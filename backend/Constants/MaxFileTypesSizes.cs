using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Constants
{
    public static class MaxFileTypesSizes
    {
        public static readonly long MaxImageSizeInBytes = 5 * 1024 * 1024;
        public static readonly long MaxDocumentSizeInBytes = 10 * 1024 * 1024;
        public static readonly long MaxVideoSizeInBytes = 100 * 1024 * 1024;
        public static readonly long MaxArchiveSizeInBytes = 50 * 1024 * 1024;

        public static readonly long MaxAudioSizeInBytes = 20 * 1024 * 1024;
    }
}