using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AzureSearchWebApiDemo.HttpResponses;
using AzureSearchWebApiDemo.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureSearchWebApiDemo.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : ApiController
    {
        private readonly ISearchIndexClient _postsIndexClient;

        public SearchController()
        {
            var searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            var apiKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];

            var searchServiceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            _postsIndexClient = searchServiceClient.Indexes.GetClient("posts");
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search(string searchText, string category)
        {
            var searchParameters = new SearchParameters
            {
                IncludeTotalResultCount = true,

                HighlightFields = new[] {"title", "description", "category"},
                HighlightPreTag = "<strong>",
                HighlightPostTag = "</strong>",

                Facets = new[] {"category"},

                ScoringProfile = "PostScoring"
            };

            if (!string.IsNullOrEmpty(category))
                searchParameters.Filter = $"category eq '{category}'";

            var response = 
                await _postsIndexClient.Documents.SearchAsync<Post>(searchText, searchParameters);

            return new HttpHttpPostActionResult(response);
        }

        [HttpGet]
        [Route("suggest")]
        public async Task<IEnumerable<Post>> Suggest(string searchText)
        {
            var suggestParameters = new SuggestParameters
            {
                Top = 5,
                UseFuzzyMatching = true // azuer => azure 'você quiz dizer...'
            };

            var response = await _postsIndexClient.Documents.SuggestAsync<Post>(searchText, "PostSuggester", suggestParameters);
            return response.Results.Select(r => r.Document);
        }

    }
}

#region Snippets

// [EnableCors(origins: "http://mvaazuresearchwebappdemo.azurewebsites.net", headers:"*", methods:"*")]


//var searchParameters = new SearchParameters
//{
//    Facets = new[] { "category" },


//    ScoringProfile = "postScoring"
//};

//            if (!string.IsNullOrEmpty(category))
//                searchParameters.Filter = $"category eq '{category}'";




#endregion