using System.Collections.Generic;
using System.Configuration;
using MvaAzureSearchDemo.AzureServices;
using MvaAzureSearchDemo.Data;
using MvaAzureSearchDemo.Model;

namespace MvaAzureSearchDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var posts = GetPosts();

            var searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"]; 
            var apiKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];

            using (var searchService = new SearchService(searchServiceName, apiKey))
                searchService.UploadDocumentsToIndex(posts);
        }




        private static IEnumerable<Post> GetPosts()
        {
            using (var postRepository = new PostRepository())
                return postRepository.All();
        }
    }
}
