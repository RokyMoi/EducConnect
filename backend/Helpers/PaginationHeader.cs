namespace EduConnect.Helpers
{
    public class PaginationHeader(int CurrentPage,int ItemsPerPage,int TotalItems,int TotalPages)
    {
        public int CurrentPage { get; set; } = CurrentPage;
        public int ItemsPerPage { get; set; }=ItemsPerPage;
        public int TotalItems { get; set; }=TotalItems;
        public int TotalPages { get; set; } = TotalPages;
    }
}
