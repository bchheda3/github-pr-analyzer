namespace github_pr_analyzer
{
    public class TeamMember
    {
        public List<string> Members { get; set; } = new List<string>();
        public List<string> Repositories { get; set; } = new List<string>();
    }

    public class TeamConfig
    {
        public string OrganizationName { get; set; } = "your-github-organization";
        public Dictionary<string, TeamMember> Teams { get; set; } = new Dictionary<string, TeamMember>();
    }
}