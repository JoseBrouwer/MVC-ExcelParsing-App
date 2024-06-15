using ExcelParsing.Controllers;
using ExcelParsing.Models;
using ExcelParsing.Services;
using ExcelParsing.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace ExcelParsing.Tests.Controllers
{
    [TestClass]
    public class ExcelParsingControllerTests
    {

        private ExcelParsingController _controller;
        private AppDbContext _context;
        private Mock<IExcelService> _mockExcelService;
        private Mock<ITempDataDictionary> _mockTempData;

        [TestInitialize]
        public void Init()
        {
            //new test DB
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
            _context = new AppDbContext(options);

            //Mock Excel Service
            _mockExcelService = new Mock<IExcelService>();

            //Controller to test
            _controller = new ExcelParsingController(_context, _mockExcelService.Object);

            //TempData for Exceptions
            _mockTempData = new Mock<ITempDataDictionary>();
            _controller.TempData = _mockTempData.Object;
        }

        [TestMethod]
        public async Task IndexViewPersons()
        {
            //Add three entries to the db for display
            var persons = new List<Person>
            {
                new Person{ID = 1, FirstName = "Jose", LastName = "Brouwer", Age = 22, status = Person.Status.Active},
                new Person{ID = 2, FirstName = "John", LastName = "Doe", Age = 23, status = Person.Status.Inactive},
                new Person{ID = 3, FirstName = "Jane", LastName = "Doe", Age = 24, status = Person.Status.Hold}
            };
            await _context.Persons.AddRangeAsync(persons);
            await _context.SaveChangesAsync();

            //Act Index with no filters
            var result = await _controller.Index(null, null, null);

            // cast Index return of Task<IActionResult> to ViewResult and check not null
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            //@model List cast to List<Person> and check for correct count of people
            var model = viewResult.Model as List<Person>;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
        }

        [TestMethod]
        public async Task UploadFileCorrectFormat()
        {
            var fileMock = new Mock<IFormFile>();
            var content = "Not Empty";
            var fileName = "test.xlsx";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);

            //Set up mock file
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            //open memory stream we wrote to
            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            //Set file name
            fileMock.Setup(f => f.FileName).Returns(fileName);
            //return length of "file"(stream)
            fileMock.Setup(f => f.Length).Returns(ms.Length);

            //call ReadExcelFile and return a list of appropriate values
            //ReadExcelFile itself is not tested here
            _mockExcelService.Setup(s => s.ReadExcelFile(It.IsAny<string>()))
                .Returns(new List<Person>
                {
                    new Person { ID = 1, FirstName = "John", LastName = "Doe", Age = 30, status = Person.Status.Active }
                });

            //provide mockFile to Upload
            var result = await _controller.UploadFile(fileMock.Object);

            //verify redirect to Index after valid upload
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod]
        public async Task UploadFileException()
        {
            var fileMock = new Mock<IFormFile>();
            var content = "Not Empty";
            var fileName = "test.xlsx";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);

            //call ReadExcelFile and resturn Exception

            _mockExcelService.Setup(s => s.ReadExcelFile(It.IsAny<string>()))
                .Throws(new Exception("Test exception"));

            //call UploadFile to get exception from ReadExcelFile
            var result = await _controller.UploadFile(fileMock.Object);

            //verify redirect to Error after invalid upload
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Error", redirectResult.ActionName);

            // Verify TempData contains the correct error message
            _mockTempData.VerifySet(tempData => tempData["ErrorMessage"] = "Test exception", Times.Once);
        }

        [TestMethod]
        public void ErrorViewWithMessage()
        {
            _mockTempData.Setup(t => t["ErrorMessage"]).Returns("Test Error Message");
            _controller.TempData = _mockTempData.Object;

            var result = _controller.Error();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);            
            Assert.AreEqual("Test Error Message", viewResult.ViewData["ErrorMessage"]);
        }
    }
}