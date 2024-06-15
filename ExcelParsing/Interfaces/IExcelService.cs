using System.Collections.Generic;
using ExcelParsing.Models;

namespace ExcelParsing.Interfaces
{
    public interface IExcelService
    {
        List<Person> ReadExcelFile(string filePath);
    }
}
