using Microsoft.EntityFrameworkCore;

namespace EduConnect.Helpers
{
    public class PagedList<T>:List<T>
    {
        public PagedList(IEnumerable<T> items,int count,int pageNumber,int pageSize)
        {
            CurrentPage = pageNumber;
            /// Logic: if we have total coumt of items of uery in repository = 15 / 5 (PageSize)= that will be Total 3 pages! :)
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
            int pageSize)
        {
            var count = await source.CountAsync();
            //So logic will be like if we have pageSize of 5, a pageNumber is 1 -1 =0, skip 0 take 5, then 2-1=1 * 5 skip first 5 take another 5..etc.
            var items = await source.Skip((pageNumber-1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count,pageNumber, pageSize);
        }
    }
}
