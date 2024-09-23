namespace EforWebApi.DTO
{
    public class EmployeeProjectDto
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public decimal EffortGoals { get; set; }
        public decimal EffortAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}