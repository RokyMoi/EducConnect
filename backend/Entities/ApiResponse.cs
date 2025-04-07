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

        public int? TotalCount { get; set; } = null;
        public int? TotalPages { get; set; } = null;
        public int? PageNumber { get; set; } = null;
        public int? PageSize { get; set; } = null;

        public static ApiResponse<T> GetApiResponse(string message, T data)
        {

            return new ApiResponse<T>(message, data);
        }

        public static ApiResponse<T> GetApiPaginatedResponse(string message, T data, int totalCount, int pageNumber, int pageSize)
        {
            return new ApiResponse<T>(message, data)
            {
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}