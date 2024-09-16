using System.Collections.Generic;
namespace EforWebApi.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool InactiveProjects { get; set; }

        // Navigation property for EmployeeProjects
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }

        // Navigation property for Efforts
        public ICollection<Effort> Efforts { get; set; }
    }
}
