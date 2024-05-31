using ExcelParsing.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExcelParsing.Controllers
{
    public class ExcelParsingController : Controller
    {
        private readonly ILogger<ExcelParsingController> _logger;

        public ExcelParsingController(ILogger<ExcelParsingController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var TestList = new List<Person>
            {
                new Person(), 
                new Person(),
                new Person(),
                new Person(),
                new Person(),
            };
            return View();
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
