using Microsoft.EntityFrameworkCore;

namespace WorkOutsAPIWebApp.Models
{
    public class WorkOutsAPIContext : DbContext
    {
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Exercises> Exercises { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WorkOuts> WorkOuts { get; set; }

        public WorkOutsAPIContext(DbContextOptions<WorkOutsAPIContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}