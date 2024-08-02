using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required]
        [Range(0, double.MaxValue)] // Ensures the salary is a non-negative value
        public decimal Salary { get; set; }

        [Required]
        public string Address { get; set; }

        public string Photo { get; set; } // Optional

        public ICollection<Order> Orders { get; set; }


    }

}
