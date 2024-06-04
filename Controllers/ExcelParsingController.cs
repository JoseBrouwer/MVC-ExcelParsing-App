using ExcelParsing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static ExcelParsing.Models.Person;


namespace ExcelParsing.Controllers
{
    public class ExcelParsingController : Controller
    {
        //private readonly ILogger<ExcelParsingController> _logger;
        //public ExcelParsingController(ILogger<ExcelParsingController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly AppDbContext _context;
        public ExcelParsingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, int page = 1, int pageSize = 50)
        {
            //_context.Persons.RemoveRange(_context.Persons);
            //await _context.SaveChangesAsync();

            ViewData["IDSort"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["FirstNameSort"] = sortOrder == "firstName" ? "firstName_desc" : "firstName";
            ViewData["LastNameSort"] = sortOrder == "lastName" ? "lastName_desc" : "lastName";
            ViewData["AgeSort"] = sortOrder == "age" ? "age_desc" : "age";
            ViewData["StatusSort"] = sortOrder == "status" ? "status_desc" : "status";

            IQueryable<Person> persons = _context.Persons;

            // Apply sorting in ascending and descending order
            switch (sortOrder)
            {
                case "id_desc":
                    persons = persons.OrderByDescending(p => p.ID);
                    break;
                case "firstName":
                    persons = persons.OrderBy(p => p.FirstName);
                    break;
                case "firstName_desc":
                    persons = persons.OrderByDescending(p => p.FirstName);
                    break;
                case "lastName":
                    persons = persons.OrderBy(p => p.LastName);
                    break;
                case "lastName_desc":
                    persons = persons.OrderByDescending(p => p.LastName);
                    break;
                case "age":
                    persons = persons.OrderBy(p => p.Age);
                    break;
                case "age_desc":
                    persons = persons.OrderByDescending(p => p.Age);
                    break;
                case "status":
                    persons = persons.OrderBy(p => p.status);
                    break;
                case "status_desc":
                    persons = persons.OrderByDescending(p => p.status);
                    break;
                default:
                    persons = persons.OrderBy(p => p.ID);
                    break;
            }

            /*
              Pagination Components: 
              (page-1)*pageSize gives the index we want to start pulling from
              Take(pageSize) will retrieve the next 50 entries as a List<Person>
            */
            var pagedPersons = await persons.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return View(pagedPersons);

            // Retrieve the list of Persons from the database
            //for (int i = 2; i >= 0; i--)
            //{
            //    Person temp = new Person { ID = i, FirstName = $"{i} First Test", LastName = $"{i} Last Test", Age = i, status = (Person.Status)i };
            //    _context.Persons.Add(temp);
            //}
            //_context.SaveChanges();
            //var persons = await _context.Persons.ToListAsync();
            //return View(persons);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
