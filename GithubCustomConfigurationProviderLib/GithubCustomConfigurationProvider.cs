using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubCustomConfigurationProviderLib
{
    public class GithubCustomConfigurationProvider : IConfigurationProvider
    {


        private readonly string _repo;
        private readonly string _filename;
        private readonly string _token;

        public GithubCustomConfigurationProvider(string repo, string filename, string token)
        {
            _repo = Environment.GetEnvironmentVariable(repo);
            Console.WriteLine(_repo);
            _filename = Environment.GetEnvironmentVariable(filename);
            Console.WriteLine(_filename);
            _token = Environment.GetEnvironmentVariable(token);
            Console.WriteLine(_token.Substring(0, 5) + "*****");
        }

        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            // Implement as required
            return new List<string>();
        }

        public void Load()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "dalisama");
            if (!string.IsNullOrEmpty(_token))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"token {_token}");
            }

            var response = client.GetAsync($"https://api.github.com/repos/{_repo}/contents/{_filename}").Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var configFile = JsonConvert.DeserializeObject<GitHubFile>(content);
                var configData = Encoding.UTF8.GetString(Convert.FromBase64String(configFile.content));
                // Convert the fetched JSON into a dictionary and set to your Data
                Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(configData);
            }
            else
            {
                Data = new Dictionary<string, string>();
            }
        }

        public void Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(string key, out string value)
        {
            return Data.TryGetValue(key, out value);
        }

        public IChangeToken GetReloadToken()
        {
            return new ConfigurationReloadToken();
        }

        public void Reload()
        {
            Load();
        }

        public IDictionary<string, string> Data { get; private set; }
    }

    public class GitHubConfigurationSource : IConfigurationSource
    {
        private readonly string _repo;
        private readonly string _filename;
        private readonly string _token;

        public GitHubConfigurationSource(string repo, string filename, string token)
        {
            _repo = repo;
            _filename = filename;
            _token = token;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new GithubCustomConfigurationProvider(_repo, _filename, _token);
        }
    }

    public class GitHubFile
    {
        public string content { get; set; }
    }

}

