using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace github_pr_analyzer
{
    public class GitHubAnalyzer
    {
        private readonly GitHubClient _githubClient;
        private readonly TeamConfig _teamConfig;

        public GitHubAnalyzer(TeamConfig teamConfig, string token)
        {
            _teamConfig = teamConfig;
            
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("GitHub token cannot be null or empty.", nameof(token));
            }

            _githubClient = new GitHubClient(new ProductHeaderValue("GitHubAnalyzer"))
            {
                Credentials = new Credentials(token)
            };
        }

        public async Task AnalyzePullRequestsAsync(DateTime fromDate, DateTime toDate)
        {
            Console.WriteLine("Starting GitHub PR analysis...");
            Console.WriteLine($"Organization: {_teamConfig.OrganizationName}");
            Console.WriteLine();

            foreach (var team in _teamConfig.Teams)
            {
                Console.WriteLine($"Analyzing team: {team.Key}");
                
                var orgName = _teamConfig.OrganizationName;

                Console.WriteLine($"  Date range: {fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}");
                Console.WriteLine($"  Members: {string.Join(", ", team.Value.Members)}");
                Console.WriteLine($"  Repositories: {string.Join(", ", team.Value.Repositories)}");
                Console.WriteLine();

                try
                {
                    var results = await AnalyzePullRequests(orgName, team.Value.Repositories, team.Value.Members, fromDate, toDate);
                    
                    Console.WriteLine($"Results for team {team.Key}:");
                    if (results.Any())
                    {
                        foreach (var result in results)
                        {
                            Console.WriteLine($"  {result.Key}: Tests: {result.Value.TestCount}, Copilot Lines: {result.Value.CopilotLineCount}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("  No results found. This could be due to:");
                        Console.WriteLine("  - No PRs in the specified date range");
                        Console.WriteLine("  - Incorrect organization name");
                        Console.WriteLine("  - Repository access permissions");
                        Console.WriteLine("  - Invalid usernames");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error analyzing team {team.Key}: {ex.Message}");
                    if (ex.Message.Contains("rate limit") || ex.Message.Contains("API rate limit"))
                    {
                        Console.WriteLine("GitHub API rate limit exceeded. Please wait and try again later.");
                    }
                    else if (ex.Message.Contains("Not Found"))
                    {
                        Console.WriteLine("Repository or organization not found. Please check your configuration.");
                    }
                }
                Console.WriteLine();
            }
        }

        public async Task<Dictionary<string, (int TestCount, int CopilotLineCount)>> AnalyzePullRequests(string orgName, List<string> repos, List<string> users, DateTime fromDate, DateTime toDate)
        {
            var teamSummary = new Dictionary<string, (int TestCount, int CopilotLineCount)>();

            foreach (var user in users)
            {
                int totalTestCount = 0;
                int totalCopilotCount = 0;

                foreach (var repo in repos)
                {
                    await EnsureRateLimitAvailable(_githubClient);

                    var searchQuery = $"repo:{orgName}/{repo} is:pr author:{user} created:{fromDate:yyyy-MM-dd}..{toDate:yyyy-MM-dd}";
                    var prRequest = new SearchIssuesRequest(searchQuery)
                    {
                        Type = IssueTypeQualifier.PullRequest
                    };

                    var searchResults = await _githubClient.Search.SearchIssues(prRequest);
                    await Task.Delay(1000); // Throttle to avoid hitting rate limit


                    foreach (var pr in searchResults.Items)
                    {
                       
                        var prFiles = await _githubClient.PullRequest.Files(orgName, repo, pr.Number);
                        await Task.Delay(1000); // Throttle

                        foreach (var file in prFiles)
                        {
                            if (file.Patch != null && file.FileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                            {
                                var copilotLines = Regex.Matches(file.Patch, @"^\+.*Added by copilot", RegexOptions.Multiline);
                                var addedTestLines = Regex.Matches(file.Patch, @"^\+.*\[Test\]", RegexOptions.Multiline);

                                totalCopilotCount += copilotLines.Count;
                                totalTestCount += addedTestLines.Count;
                            }
                        }
                    }
                }

                teamSummary[user] = (totalTestCount, totalCopilotCount);
            }

            return teamSummary;
        }

        static async Task EnsureRateLimitAvailable(GitHubClient github)
        {
            var rateLimit = await github.RateLimit.GetRateLimits();
            var core = rateLimit.Rate;

            if (core.Remaining < 10)
            {
                var waitTime = core.Reset.UtcDateTime - DateTime.UtcNow;
                Console.WriteLine($"Rate limit exceeded. Waiting {waitTime.TotalMinutes:F1} minutes until reset at {core.Reset.UtcDateTime} UTC...");
                await Task.Delay(waitTime + TimeSpan.FromSeconds(5));
            }
        }
    }
}