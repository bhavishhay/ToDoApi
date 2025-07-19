namespace ToDoApi.Application.QueryParameters
{
    public class ToDoQuaryFilterSortingParameters : QuaryPagingParameters
    {
        public string? Title { get; set; }
        public bool? Status { get; set; }
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; } = false;
    }
}
