namespace ToDoApi.Models
{
    public class UserQuaryFilterSortingParameters : QuaryPagingParameters
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; } = false;
    }
}
