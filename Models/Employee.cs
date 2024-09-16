using EforWebApi.Models;
using System.Collections.Generic;

namespace EforWebApi.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Groups { get; set; }
        public DateTime HireDate { get; set; }
        public string Password { get; set; }
        public int MonthlyEfor { get; set; }
        public bool InactiveEmployees { get; set; }

        // Navigation property for EmployeeProjects
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }

        // Navigation property for Efforts
        public ICollection<Effort> Efforts { get; set; }
    }
}
