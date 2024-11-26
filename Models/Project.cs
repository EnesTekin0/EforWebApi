using System.Collections.Generic;
namespace EforWebApi.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public required string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ActiveProjects { get; set; }

        public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}