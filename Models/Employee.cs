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
        public required string Groups { get; set; }
        public DateTime HireDate { get; set; }
        public bool ActiveEmployees { get; set; }

        public string Role { get; set; } = "User";

        public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}