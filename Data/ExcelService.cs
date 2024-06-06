using ExcelDataReader;
using ExcelParsing.Models;
using System.IO;

namespace ExcelParsing.Data
{
    public class ExcelService
    {
        public List<Person> ReadExcelFile(string filePath)
        {
            //Handles possible issues with text encoding
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var persons = new List<Person>();

            //Auto-detects .xls, .xlsx, .xlsb files
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet();
                    var dataTable = dataSet.Tables[0];

                    for (int row = 1; row < dataTable.Rows.Count; row++)
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
                }
            }
            return persons;
        }
    }
}
