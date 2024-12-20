using Microsoft.EntityFrameworkCore;

namespace EduConnect.Helpers
{
    public class PagedList<T>:List<T>
    {
        public PagedList(IEnumerable<T> items,int count,int pageNumber,int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count/(double) PageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
            
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateASync(
            IQueryable<T> source,
            int pageNumber,
            int pagesize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber-1) * pagesize).Take(pageNumber).ToListAsync();
            return new PagedList<T>(items, count,pageNumber, pagesize);
        }
    }
}
