using System.ComponentModel.DataAnnotations.Schema;

namespace EforWebApi.Models
{
    public class EmployeeProject
    {
        public int EmployeeProjectId { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public decimal EffortGoals { get; set; }
        public decimal EffortAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        ////Navigation property for EmployeeProjects 
        //public virtual ICollection<Effort> Effort { get; set; } = new List<Effort>();

        //// Foreign key for EmployeeProject 
        //[ForeignKey("EmployeeId")]
        //public virtual Employee Employee { get; set; }

        //[ForeignKey("ProjectId")]
        //public virtual Project Project { get; set; }
    }
}
