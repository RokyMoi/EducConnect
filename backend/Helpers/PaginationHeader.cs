namespace EduConnect.Helpers
{
    public class PaginationHeader(int currentPage,int itemsPerPage,int totalItems,int totalPages)
    {
        /// Making a pagination header for response extension, to send to a client <summary>
       //Response.AddPaginationHeader - > Extensions Folder
        public int CurrentPage { get; set; } = currentPage;
        public int ItemsPerPage { get; set; }=itemsPerPage;
        public int TotalItems { get; set; }=totalItems;
        public int TotalPages { get; set; } = totalPages;
    }
}
