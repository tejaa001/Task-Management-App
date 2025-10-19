using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
     // Declares the TodoContext class which inherits from DbContext for database operations
    public class TodoContext : DbContext
    {
        // Constructor that accepts DbContext options and passes them to the base class
        public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
        {
        }

        // Property representing a collection of Todo entities in the database
        public DbSet<Todo> Todos { get; set; } = null!;
    }
}
