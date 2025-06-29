using System;
using System.Collections.Generic;

namespace Clinic_Management_System.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal? Salary { get; set; }
    }
}
