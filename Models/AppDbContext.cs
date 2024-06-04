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
    }
}
