using Microsoft.Extensions.Configuration;

namespace GithubCustomConfigurationProviderLib
{
    public static class GitHubConfigurationExtensions
    {
        public static IConfigurationBuilder AddGitHubConfiguration(this IConfigurationBuilder builder, string repo, string filename, string token)
        {
            return builder.Add(new GitHubConfigurationSource(repo, filename, token));
        }
    }

}

