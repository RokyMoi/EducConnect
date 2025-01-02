using EduConnect.Helpers;
using Microsoft.AspNetCore.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace EduConnect.Extensions
{
    public static class HttpExtensions
    { 
       //Making a Type T so we can send, a more types  Member,Course
    
        public static void AddPaginationHeader<T>(this HttpResponse response,PagedList<T> data)
        {
            var paginationHeader = new PaginationHeader(data.CurrentPage, data.PageSize,data.TotalCount,data.TotalPages);
            //Creating json options - CamelCase returning in json, standard format
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            response.Headers.Append("Pagination",JsonSerializer.Serialize(paginationHeader,jsonOptions));
            //Cors header to allow client have access for header up <-if we dont have this one, then this header will not be visible to client
            //Or if we spell wront Pagination,wrong - casing or something.
            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
        }

        }
    }

