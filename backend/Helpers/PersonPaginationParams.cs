namespace EduConnect.Helpers
{
    public class PersonPaginationParams
    {
        //User parameter for client needs, -> Default maximum.
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public int _pageSize { get; set; } = 10;
        //snippet: propfull - For this type of prop format.
        public int PageSize { 
            get=>_pageSize;
            
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        
        
        }
    }
}
