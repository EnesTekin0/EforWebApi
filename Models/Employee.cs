using EforWebApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EforWebApi.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Groups { get; set; }
        public DateTime HireDate { get; set; }
        public bool InactiveEmployees { get; set; }

        ////Navigation property for EmployeeProjects  Çalışıyor
        //public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}
