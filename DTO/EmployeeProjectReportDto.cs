public class EmployeeProjectReportDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<ProjectEffortDetail> Projects { get; set; }
}

public class ProjectEffortDetail
{
    public string ProjectName { get; set; }
    public List<MonthlyEffort> MonthlyEfforts { get; set; }
}

public class MonthlyEffort
{
    public int Month { get; set; }
    public decimal EffortAmount { get; set; }
}