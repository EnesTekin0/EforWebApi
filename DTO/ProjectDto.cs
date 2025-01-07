namespace EforWebApi.DTO
{
    public class ProjectDto
    {
        public required string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string GitHubLink { get; set; }
        public string JiraLink { get; set; }
        public string ProdLink { get; set; }
        public string PreProdLink { get; set; }
        public string TestLink { get; set; }
        public bool ActiveProjects { get; set; }
    }
}