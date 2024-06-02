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

        public async Task<IActionResult> Index()
        {
            _context.Persons.RemoveRange(_context.Persons);
            await _context.SaveChangesAsync();

            // Retrieve the list of Persons from the database
            for (int i = 0; i < 3; i++)
            {
                Person temp = new Person { ID = i, FirstName = "First Test", LastName = "Last Test", Age = 22, status = (Person.Status)i };
                _context.Persons.Add(temp);
            }
            _context.SaveChanges();
            var persons = await _context.Persons.ToListAsync();
            return View(persons);
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
