namespace ToDoApi.Application.QueryParameters
{
    public class QuaryPagingParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public QuaryPagingParameters()
        {
            
            if (PageSize > 15)
            {
                PageSize = 15; // Limit the maximum page size to 15
            }
            if (PageNumber < 1)
            {
                PageNumber = 1; // Ensure page number is at least 1
            }
        }
    }
}
