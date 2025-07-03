# Setup Guide for GitHub PR Analyzer

## Introduction
This guide provides step-by-step instructions for setting up and running the GitHub PR Analyzer application. The application analyzes GitHub Pull Requests to track test additions and Copilot contributions across development teams. You do not need to have a technical background to follow these instructions.

## Prerequisites
Before you begin, ensure you have the following:

1. **.NET 8 SDK**: Download and install from the official [.NET website](https://dotnet.microsoft.com/download)
2. **GitHub Personal Access Token**: You'll need this to access GitHub data (instructions provided below)
3. **Access to your organization's repositories**: Ensure you have permission to view the repositories you want to analyze

## Step 1: Download and Prepare the Project
1. Clone or download the GitHub PR Analyzer project from the repository
2. Extract the files to a folder on your computer (e.g., `C:\github-pr-analyzer`)
3. Open a command prompt or terminal and navigate to the project folder:
   ```bash
   cd path/to/github-pr-analyzer
   ```

## Step 2: Create a GitHub Personal Access Token
1. Go to [GitHub.com](https://github.com) and sign in to your account
2. Click on your profile picture in the top-right corner
3. Select **Settings** from the dropdown menu
4. In the left sidebar, scroll down and click **Developer settings**
5. Click **Personal access tokens** → **Tokens (classic)**
6. Click **Generate new token (classic)**
7. Give your token a descriptive name (e.g., "PR Analyzer Tool")
8. Select the following scopes/permissions:
   - ✅ **repo** (Full control of private repositories)
   - ✅ **user** (Read user profile data)
9. Click **Generate token**
10. **Important**: Copy the token immediately and save it securely - you won't be able to see it again!

## Step 3: Configure Team Information
1. Navigate to the `config` folder in the project directory
2. Open the `teams.json` file in a text editor (Notepad, VS Code, etc.)
3. Update the file with your organization and team information:

```json
{
  "organizationName": "your-github-organization-name",
  "teams": {
    "YourTeamName": {
      "members": [
        "github-username1",
        "github-username2",
        "github-username3"
      ],
      "repositories": [
        "repository-name-1",
        "repository-name-2",
        "repository-name-3"
      ]
    }
  }
}
```

**Replace the following:**
- `your-github-organization-name`: Your actual GitHub organization name
- `YourTeamName`: A descriptive name for your team
- `github-username1`, etc.: Actual GitHub usernames of team members
- `repository-name-1`, etc.: Actual repository names you want to analyze

## Step 4: Build the Application
1. In your command prompt/terminal (in the project directory), run:
   ```bash
   dotnet build
   ```
2. Wait for the build to complete successfully

## Step 5: Run the Application
1. Start the application by running:
   ```bash
   dotnet run
   ```

2. The application will prompt you for information:

   **a) GitHub Token:**
   ```
   Enter your GitHub Personal Access Token:
   ```
   Paste the token you created in Step 2 and press Enter

   **b) Date Range:**
   ```
   From date (yyyy-mm-dd) [default: 30 days ago]:
   ```
   - Press Enter to use the default (30 days ago), OR
   - Type a specific date in yyyy-mm-dd format (e.g., `2025-06-01`)

   ```
   To date (yyyy-mm-dd) [default: today]:
   ```
   - Press Enter to use today's date, OR
   - Type a specific end date in yyyy-mm-dd format (e.g., `2025-07-03`)

3. The application will start analyzing and display progress information

## Step 6: Review Results
The application will display results like this:
```
=== GitHub PR Analyzer ===

Analysis period: 2025-06-03 to 2025-07-03

Starting GitHub PR analysis...
Organization: YourOrganization

Analyzing team: YourTeamName
  Date range: 2025-06-03 to 2025-07-03
  Members: user1, user2, user3
  Repositories: repo1, repo2, repo3

Results for team YourTeamName:
  user1: Tests: 15, Copilot Lines: 42
  user2: Tests: 8, Copilot Lines: 31
  user3: Tests: 12, Copilot Lines: 28

Analysis complete. Check the output for results.
```

## Troubleshooting

### Common Issues:
1. **"Bad credentials"**: Your GitHub token is invalid or expired - generate a new one
2. **"Repository not found"**: Check your organization name and repository names in `teams.json`
3. **"Rate limit exceeded"**: Wait a few minutes and try again - GitHub limits API requests
4. **Build errors**: Ensure .NET 8 SDK is properly installed

### Tips:
- Keep your GitHub token secure and don't share it
- Start with a smaller date range if you have many repositories
- The application analyzes only `.cs` files (C# code files)
- Test counts look for `[Test]` attributes in added lines
- Copilot counts look for "Added by copilot" comments in added lines

## Support
If you encounter issues not covered in this guide, please refer to the README.md file for additional technical information.