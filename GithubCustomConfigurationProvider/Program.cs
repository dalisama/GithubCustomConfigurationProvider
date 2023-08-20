using Microsoft.Extensions.Configuration;
using GithubCustomConfigurationProviderLib;

namespace GithubCustomConfigurationProvider
{

    internal class Program
    {
        static void Main(string[] args)

        {
            Console.WriteLine("Hello, World!");
            Environment.SetEnvironmentVariable("REPO", "GithubCustomConfigurationProvider");
            Environment.SetEnvironmentVariable("FILENAME", "jsconfig.json");
            Environment.SetEnvironmentVariable("TOKEN", "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");//read only token

            IConfiguration Configuration = new ConfigurationBuilder()
                         .AddGitHubConfiguration("REPO", "FILENAME", "TOKEN")
                         .Build();



        }
    }
}