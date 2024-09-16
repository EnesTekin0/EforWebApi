namespace EforWebApi.Models
{
    public class Effort
    {
        public int EffortId { get; set; }
        public DateTime EfforDate { get; set; }
        public int Amount { get; set; }

        // Foreign key for Employee
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        // Foreign key for Project
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
