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
