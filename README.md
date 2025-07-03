# GitHub PR Analyzer

A .NET 8 console application that analyzes GitHub Pull Requests to track test additions and potential Copilot contributions across development teams.

## Features

- Analyzes Pull Requests for specified teams and repositories
- Tracks test additions (looking for `[Test]` attributes)
- Counts potential Copilot-generated tests (looking for "Added by copilot" comments)
- Configurable team and repository settings
- Rate limiting awareness for GitHub API

## Prerequisites

- .NET 8 SDK
- GitHub Personal Access Token (for accessing GitHub API)

## Setup

### 1. Clone and Build

```bash
git clone <repository-url>
cd github-pr-analyzer
dotnet build
```

### 2. Configure Teams

Edit `config/teams.json` to configure your teams and repositories:

```json
{
  "organizationName": "your-github-organization",
  "teams": {
    "TeamName": {
      "members": [
        "github-username1",
        "github-username2"
      ],
      "repositories": [
        "repo-name-1",
        "repo-name-2"
      ]
    }
  }
}
```

### 3. Generate GitHub Token

Create a GitHub Personal Access Token:
1. Go to GitHub Settings > Developer settings > Personal access tokens > Tokens (classic)
2. Generate a new token with `repo` and `user` scopes

## Usage

```bash
dotnet run
```

The application will:
1. Load team configuration from `config/teams.json`
2. Analyze Pull Requests for the given period
3. Generate reports showing test additions and Copilot test counts per team member

## Sample Output

```
Analyzing team: FantasticFour
Results for team FantasticFour:
  rk5593: Tests: 15, Copilot Lines: 42
  saj108: Tests: 8, Copilot Lines: 31
  hdkumar: Tests: 12, Copilot Lines: 28
  vksharma: Tests: 6, Copilot Lines: 19
```

## Troubleshooting

### Common Issues

1. **Rate limiting**: The GitHub API has rate limits. The application handles basic rate limiting but may need delays for large datasets
2. **Repository access**: Ensure your token has access to the repositories you're trying to analyze

### Demo Mode

If no GitHub token is provided, the application runs in demo mode and shows setup instructions.

## Dependencies

- **Newtonsoft.Json**: For JSON configuration parsing
- **Octokit**: GitHub API client library
- **.NET 8**: Target framework

## License

[Add your license information here]