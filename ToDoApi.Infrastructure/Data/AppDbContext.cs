using Microsoft.EntityFrameworkCore;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add Index on Email in User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Add Index on Title in ToDo
            modelBuilder.Entity<ToDo>()
                .HasIndex(t => t.Title);
        }

    }
}
