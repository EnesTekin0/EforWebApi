using System.Collections.Generic;
namespace EforWebApi.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public required string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool InactiveProjects { get; set; }

        ////Navigation property for EmployeeProjects  Çalışıyor
        //public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}
