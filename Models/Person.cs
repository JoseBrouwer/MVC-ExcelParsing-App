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


        public enum Status
        {
            Active,
            Inactive,
            Hold
        }
        [Required]
        public Status status{ get; set; }

        public Person(int iD = 0, string firstName = " First Test", string lastName = " Last Test", int age = 21, Status state = 0)
        {
            ID = iD;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            status = state;
        }
    }
}
