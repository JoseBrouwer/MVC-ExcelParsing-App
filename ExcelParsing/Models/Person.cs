using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelParsing.Models
{
    public class Person
    {
        [Key] //Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.None)]  // Disable automatic value generation
        public int ID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Age must be a positive number.")]
        public int Age { get; set; }


        public enum Status
        {
            Active,
            Inactive,
            Hold
        }
        [Required]
        public Status status{ get; set; }

        public Person() { }
        public Person(int id, string firstName, string lastName, int age, Status state)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            status = state;
        }
    }
}
