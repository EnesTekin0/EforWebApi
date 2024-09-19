using System.ComponentModel.DataAnnotations.Schema;

namespace EforWebApi.Models
{
    public class Effort
    {
        public int EffortId { get; set; }
        public int EmployeeProjectId { get; set; }
        public DateTime EffortDate { get; set; }
        public decimal MonthlyEffort { get; set; }

        //[ForeignKey("EmployeeProjectId")]
        //public virtual EmployeeProject EmployeeProject { get; set; }
    }
}
