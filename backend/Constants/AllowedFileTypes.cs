using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Constants
{
    public static class AllowedFileTypes
    {
        public static readonly string[] Documents = {
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.ms-powerpoint",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            "application/vnd.ms-excel",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "text/plain",
            "application/rtf"
        };

        public static readonly string[] Images = {
            "image/png",
            "image/jpeg",
            "image/jpg",
            "image/gif"
        };

        public static readonly string[] Videos = {
            "video/mp4",
            "video/mpeg",
            "video/quicktime",
            "video/x-msvideo",
            "video/x-ms-wmv",
            "video/x-matroska"
        };

        public static readonly string[] Archives = {
            "application/zip",
            "application/x-rar-compressed",
            "application/x-rar"
        };

        public static readonly string[] Audio = {
            "audio/mpeg",
            "audio/wav",
            "audio/ogg",
            "audio/flac",
            "audio/x-ms-wma"
        };

    }
}