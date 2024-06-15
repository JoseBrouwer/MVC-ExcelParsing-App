using ExcelParsing.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelParsing.Data
{
    public static class DatabaseSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                // Check if the database is already seeded
                if (context.Persons.Any())
                {
                    return;   // DB has been seeded
                }

                // Seed the database with test data
                for (int i = 0; i < 100; i++)
                {
                    Person temp = new Person
                    {
                        ID = i,
                        FirstName = $"{i} First Test",
                        LastName = $"{i} Last Test",
                        Age = i,
                        status = (Person.Status)(i % 3)
                    };
                    context.Persons.Add(temp);
                }

                context.SaveChanges();
            }
        }
    }
}

