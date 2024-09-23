namespace EforWebApi.DTO
{
    public class ProjectDto
    {
        public required string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool InactiveProjects { get; set; }
    }
}