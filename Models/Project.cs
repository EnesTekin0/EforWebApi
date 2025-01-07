using System.Collections.Generic;
namespace EforWebApi.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public required string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string GitHubLink { get; set; } 
        public string JiraLink { get; set; } 
        public string ProdLink { get; set; } 
        public string PreProdLink { get; set; } 
        public string TestLink { get; set; } 
        public bool ActiveProjects { get; set; }

        public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}