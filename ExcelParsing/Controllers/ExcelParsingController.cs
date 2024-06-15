using ExcelParsing.Models;
using ExcelParsing.Services;
using ExcelParsing.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Xml.Linq;
using static ExcelParsing.Models.Person;


namespace ExcelParsing.Controllers
{
    public class ExcelParsingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IExcelService _excelService;
        public ExcelParsingController(AppDbContext context, IExcelService excelService)
        {
            _context = context;
            _excelService = excelService;
        }

        public async Task<IActionResult> Index(string sortOrder, string name, string status, int page = 1, int pageSize = 50)
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
            ViewData["StatusSort"] = sortOrder == "active" ? "inactive" : (sortOrder == "inactive" ? "hold" : "active");


            IQueryable<Person> persons = _context.Persons;

            if (!string.IsNullOrEmpty(name))
            {
                name = char.ToUpper(name[0]) + name.Substring(1);
                persons = persons.Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name));
            }
            else if (!string.IsNullOrEmpty(status))
            {
                status = char.ToUpper(status[0]) + status.Substring(1);
                persons = persons.Where(p => p.status.ToString().Contains(status));
            }

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
                case "active":
                    //set order by number. If status is Active = 0. Then, status Inactive = 1. Else, Hold = 2.
                    //OrderBy the sorts as Active, Inactive, Else
                    persons = persons.OrderBy(p => p.status == Person.Status.Active ? 0 : p.status == Person.Status.Inactive ? 1 : 2);
                    break;
                case "inactive":
                    persons = persons.OrderBy(p => p.status == Person.Status.Inactive ? 0 : p.status == Person.Status.Hold ? 1 : 2);
                    break;
                case "hold":
                    persons = persons.OrderBy(p => p.status == Person.Status.Hold ? 0 : p.status == Person.Status.Active ? 1 : 2);
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
                //clear db for new upload
                _context.Persons.RemoveRange(_context.Persons);
                await _context.SaveChangesAsync();

                //retrieve file path
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream); //copy file contents to file stream
                }
                try
                {
                    //Call function that reads from stream and creates Person objects
                    var persons = _excelService.ReadExcelFile(filePath);
                    _context.Persons.AddRange(persons);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex) //reading file failed
                {
                    TempData["ErrorMessage"] = ex.Message; // Log error
                    return RedirectToAction("Error"); //redirect to error page
                }
            }
            return RedirectToAction(nameof(Index)); //reload page
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
