using System;
using System.Threading.Tasks;

namespace github_pr_analyzer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("=== GitHub PR Analyzer ===");
            Console.WriteLine();

            // Get GitHub token from user
            Console.Write("Enter your GitHub Personal Access Token: ");
            var token = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Error: GitHub token is required to run the analysis.");
                Console.WriteLine("Please restart the application and provide a valid token.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            // Get date range from user
            Console.WriteLine();
            Console.WriteLine("Enter date range for analysis:");
            
            DateTime fromDate;
            while (true)
            {
                Console.Write("From date (yyyy-mm-dd) [default: 30 days ago]: ");
                var fromDateInput = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(fromDateInput))
                {
                    fromDate = DateTime.Now.AddDays(-30);
                    break;
                }
                
                if (DateTime.TryParse(fromDateInput, out fromDate))
                {
                    break;
                }
                
                Console.WriteLine("Invalid date format. Please use yyyy-mm-dd format.");
            }

            DateTime toDate;
            while (true)
            {
                Console.Write("To date (yyyy-mm-dd) [default: today]: ");
                var toDateInput = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(toDateInput))
                {
                    toDate = DateTime.Now;
                    break;
                }
                
                if (DateTime.TryParse(toDateInput, out toDate))
                {
                    break;
                }
                
                Console.WriteLine("Invalid date format. Please use yyyy-mm-dd format.");
            }

            Console.WriteLine();
            Console.WriteLine($"Analysis period: {fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}");
            Console.WriteLine();

            // Load team configuration
            var teamConfig = ConfigHelper.LoadTeamConfig("config/teams.json");

            // Initialize GitHub Analyzer with token
            var githubAnalyzer = new GitHubAnalyzer(teamConfig, token);

            // Start the analysis process with date range
            await githubAnalyzer.AnalyzePullRequestsAsync(fromDate, toDate);

            Console.WriteLine("Analysis complete. Check the output for results.");
        }
    }
}