namespace EforWebApi.Models
{
    public class EmployeeProject
    {
        public int EmployeeProjectId { get; set; }
        public DateTime EfforDate { get; set; }
        public string Goals { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Foreign key for Employee
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        // Foreign key for Project
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
