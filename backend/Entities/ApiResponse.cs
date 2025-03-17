using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities
{
    public class ApiResponse<T>(string message, T data)
    {
        public string Message { get; set; } = message;
        public T Data { get; set; } = data;
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public static ApiResponse<T> GetApiResponse(string message, T data)
        {

            return new ApiResponse<T>(message, data);
        }
    }
}