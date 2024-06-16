using ExcelParsing.Interfaces;
using ExcelParsing.Models;
using ExcelParsing.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace ExcelParsing.Tests.Services
{
    [TestClass]
    public class ExcelServiceTests
    {
        private IExcelService _excelService;
        private readonly string _testFilesDir;

        public ExcelServiceTests()
        {
            // Set the base directory to the project directory
            var baseDirectory = AppContext.BaseDirectory; //..\bin\Debug\net8.0
            var projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\..\ExcelParsing.Tests"));
            _testFilesDir = Path.Combine(projectDirectory, "Data");
        }

        [TestInitialize]
        public void Setup()
        {
            _excelService = new ExcelService();
        }

        [TestMethod]
        public void ReadExcelFileValid()
        {
            var filePath = Path.Combine(_testFilesDir, "ValidData.xlsx");

            var result = _excelService.ReadExcelFile(filePath);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Person>)); //returns list
            Assert.AreEqual(5, result.Count); //list has 5 entries
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ReadExcelFileException()
        {
            var filePath = Path.Combine(_testFilesDir, "InvalidData.xlsx");
            _excelService.ReadExcelFile(filePath); //will throw exception
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ReadExcelFileNotFound()
        {
            var filePath = Path.Combine(_testFilesDir, "nonExistentTest.xlsx");
            _excelService.ReadExcelFile(filePath);
        }
    }
}