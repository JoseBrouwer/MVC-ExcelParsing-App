using Microsoft.EntityFrameworkCore;
using static ExcelParsing.Models.Person;

namespace ExcelParsing.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("ExcelParsingDB");
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Seed data
        //    modelBuilder.Entity<Person>().HasData(
        //        new Person { ID = 1, FirstName = "John", LastName = "Doe", Age = 30, status = Status.Active },
        //        new Person { ID = 2, FirstName = "Jane", LastName = "Smith", Age = 25, status = Status.Inactive },
        //        new Person { ID = 3, FirstName = "Alice", LastName = "Johnson", Age = 40, status = Status.Hold },
        //        new Person { ID = 4, FirstName = "Bob", LastName = "Brown", Age = 35, status = Status.Active }
        //    );
        //}
    }
}
