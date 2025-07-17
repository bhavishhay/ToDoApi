using Microsoft.EntityFrameworkCore;

namespace ToDoApi.Models
{
   // [Index(nameof(Title))] // Index on Title for faster queries by DataAnnotations
    public class ToDo
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; } = false;
    }
}