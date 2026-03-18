using Microsoft.EntityFrameworkCore;

namespace MovieApp.Models
{
    public class AppDbContext : DbContext
    {
        // This line tells EF Core to create an "Events" table in SQL
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MovieEventsDB;Trusted_Connection=True;");
        }
    }
}