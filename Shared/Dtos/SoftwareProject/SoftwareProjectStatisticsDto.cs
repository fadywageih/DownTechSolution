namespace Shared.Dtos.SoftwareProject
{
    public class SoftwareProjectStatisticsDto
    {
        public int TotalProjects { get; set; }
        public Dictionary<string, int> ProjectsByFrontendType { get; set; } = new();
        public Dictionary<string, int> ProjectsByBackendType { get; set; } = new();
        public int TotalAngularProjects { get; set; }
        public int TotalReactProjects { get; set; }
        public int TotalDotNetProjects { get; set; }
        public int TotalNodeProjects { get; set; }
        public int TotalPythonProjects { get; set; }
        public DateTime LatestProjectDate { get; set; }
    }
}
