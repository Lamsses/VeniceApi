﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Dto
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        
        public int RandomId { get; set; } 
        public string Name { get; set; }

        public string JobTitle { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string? Password { get; set; }

        public decimal Salary { get; set; }
        public bool IsVisible { get; set; } = true;


        public string Address { get; set; }

        public string Photo { get; set; }
    }

    
}