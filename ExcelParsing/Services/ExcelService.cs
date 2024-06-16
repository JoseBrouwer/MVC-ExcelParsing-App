using ExcelDataReader;
using ExcelParsing.Models;
using ExcelParsing.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace ExcelParsing.Services
{
    public class ExcelService : IExcelService
    {
        public List<Person> ReadExcelFile(string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var persons = new List<Person>();

            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var dataSet = reader.AsDataSet();
                        var dataTable = dataSet.Tables[0];

                        for (int row = 1; row < dataTable.Rows.Count; row++)
                        {
                            try
                            {
                                //check for issues before assignment
                                string? firstValue = dataTable.Rows[row][1].ToString();
                                if (firstValue == null || firstValue?.Length > 50)
                                    throw new FormatException($"First name can not be longer than 50 characters");

                                string? lastValue = dataTable.Rows[row][2].ToString();
                                if (lastValue == null || lastValue?.Length > 50)
                                    throw new FormatException($"Last name can not be longer than 50 characters");

                                //default to age=0 if trying to parse a null
                                int ageValue = int.Parse(dataTable.Rows[row][3].ToString() ?? "0");
                                if (ageValue < 0)
                                    throw new FormatException($"Age can not be a negative number");

                                var person = new Person
                                {
                                    //default to ID=0 if null which will throw an exception if more than one ID=0
                                    ID = int.Parse(dataTable.Rows[row][0].ToString() ?? "0"),
                                    FirstName = firstValue,
                                    LastName = lastValue,
                                    Age = ageValue,
                                    //default status=Hold if null
                                    status = (Person.Status)Enum.Parse(typeof(Person.Status), dataTable.Rows[row][4].ToString() ?? "2")
                                };
                                persons.Add(person);
                            }
                            catch (FormatException ex)
                            {
                                // Handle specific format exception
                                throw new FormatException($"Data format error in row {row + 1} -> {ex.Message}", ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log and rethrow the exception to be handled by global exception handler
                if (ex is FormatException)
                    throw new FormatException($"{ex.Message}", ex);
                else if (ex is FileNotFoundException)
                    throw new FileNotFoundException($"{ex.Message}", ex);
            }

            return persons;
        }
    }
}
