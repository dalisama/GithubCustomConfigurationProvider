using Microsoft.Extensions.Configuration;
using GithubCustomConfigurationProviderLib;

namespace GithubCustomConfigurationProvider
{

    internal class Program
    {
        static void Main(string[] args)

        {
            Console.WriteLine("Hello, World!");
            Environment.SetEnvironmentVariable("REPO", "dalisama/GithubCustomConfigurationProvider");
            Environment.SetEnvironmentVariable("FILENAME", "ConfigurationData/jsconfig.json");
            Environment.SetEnvironmentVariable("TOKEN", "");//read only token

            IConfiguration Configuration = new ConfigurationBuilder()
                         .AddGitHubConfiguration("REPO", "FILENAME", "TOKEN")
                         .Build();

            var testdata = Configuration.GetSection("testdata");

            Console.Read();

        }
    }
}