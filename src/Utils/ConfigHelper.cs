using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace github_pr_analyzer

{
    public static class ConfigHelper
    {
        public static TeamConfig LoadTeamConfig(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Configuration file not found: {filePath}");
            }

            var json = File.ReadAllText(filePath);
            var result = JsonConvert.DeserializeObject<TeamConfig>(json);
            return result ?? throw new InvalidOperationException("Failed to deserialize team configuration");
        }
    }
}