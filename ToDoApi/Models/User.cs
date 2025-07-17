using Microsoft.EntityFrameworkCore;

namespace ToDoApi.Models
{
    // [Index(nameof(Email), IsUnique = true)] // Index on Title for faster queries by DataAnnotations
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        
    }
}
