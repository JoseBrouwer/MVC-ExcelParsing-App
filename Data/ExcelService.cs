using ExcelDataReader;
using ExcelParsing.Models;
using System.IO;

namespace ExcelParsing.Data
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
                                var person = new Person
                                {
                                    ID = int.Parse(dataTable.Rows[row][0].ToString()),
                                    FirstName = dataTable.Rows[row][1].ToString(),
                                    LastName = dataTable.Rows[row][2].ToString(),
                                    Age = int.Parse(dataTable.Rows[row][3].ToString()),
                                    status = (Person.Status)Enum.Parse(typeof(Person.Status), dataTable.Rows[row][4].ToString())
                                };
                                persons.Add(person);
                            }
                            catch (FormatException ex)
                            {
                                // Handle specific format exception
                                throw new Exception($"Data format error in row {row + 1}: {ex.Message}", ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log and rethrow the exception to be handled by global exception handler
                throw new Exception($"Error reading Excel file: {ex.Message}", ex);
            }

            return persons;
        }
    }
}
