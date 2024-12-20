using EduConnect.Helpers;
using Microsoft.AspNetCore.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace EduConnect.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader<T>(this HttpResponse response,PagedList<T> data)
        {
            var paginationHeader = new PaginationHeader(data.CurrentPage, data.PageSize,data.TotalCount,data.TotalPages);
            //Access for client for pagination 
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            response.Headers.Append("Pagination",JsonSerializer.Serialize(paginationHeader,jsonOptions));
            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
        }

        }
    }
}
