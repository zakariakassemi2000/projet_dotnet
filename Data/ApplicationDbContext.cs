using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoTask> TodoTasks { get; set; }
}
