using System.ComponentModel.DataAnnotations;

namespace ExcelParsing.Models
{
    public class Person
    {
        [Key] //Primary Key
        public int ID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Age must be a positive number.")]
        public int Age { get; set; }

        [Required]
        [RegularExpression("Active|Inactive|Hold", ErrorMessage = "Status must be Active, Inactive, or Hold.")]
        public string Status { get; set; }
    }
}
