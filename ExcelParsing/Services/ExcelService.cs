using ExcelDataReader;
using ExcelParsing.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace ExcelParsing.Services
{
    public class ExcelService
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

                                int ageValue = int.Parse(dataTable.Rows[row][3].ToString());
                                if (ageValue < 0)
                                    throw new FormatException($"Age can not be a negative number");

                                var person = new Person
                                {
                                    ID = int.Parse(dataTable.Rows[row][0].ToString()),
                                    FirstName = firstValue,
                                    LastName = lastValue,
                                    Age = ageValue,
                                    status = (Person.Status)Enum.Parse(typeof(Person.Status), dataTable.Rows[row][4].ToString())
                                };
                                persons.Add(person);
                            }
                            catch (FormatException ex)
                            {
                                // Handle specific format exception
                                throw new Exception($"Data format error in row {row + 1} -> {ex.Message}", ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log and rethrow the exception to be handled by global exception handler
                throw new Exception($"{ex.Message}", ex);
            }

            return persons;
        }
    }
}
