using ExcelParsing.Data;
using ExcelParsing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static ExcelParsing.Models.Person;


namespace ExcelParsing.Controllers
{
    public class ExcelParsingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ExcelService _excelService;
        public ExcelParsingController(AppDbContext context, ExcelService excelService)
        {
            _context = context;
            _excelService = excelService;
        }

        public async Task<IActionResult> Index(string sortOrder, int page = 1, int pageSize = 50)
        {
            int totalEntries = await _context.Persons.CountAsync(); // Retrieve the total count of entries in the database
            int remainder = totalEntries % pageSize;
            int totalPages = (int)Math.Ceiling(totalEntries / (double)pageSize);
            ViewData["TotalEntries"] = totalEntries;
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;
            ViewData["CurrentSort"] = sortOrder;

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
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            //file is provided
            if (file != null && file.Length > 0)
            {
                //retrieve file path
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream); //copy file contents to file stream
                }
                //Call function that reads from stream and creates Person objects
                var persons = _excelService.ReadExcelFile(filePath);
                _context.Persons.AddRange(persons); //add to DB
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); //reload page
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
