using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using AzureSearchWebApiDemo.Models;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;

namespace AzureSearchWebApiDemo.HttpResponses
{
    public class HttpHttpPostActionResult : IHttpActionResult
    {
        public long? Count { get; private set; }
        public IList<Post> Data { get; private set; }
        public IList<FacetResult> Facets { get; private set; }

        public HttpHttpPostActionResult(DocumentSearchResponse<Post> searchResponse)
        {
            Count = searchResponse.Count;
            Facets = searchResponse.Facets?["category"];
            Data = Post.ToModels(searchResponse.Results);
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage
            {
                Content =
                    new StringContent(JsonConvert.SerializeObject(this), Encoding.Default,
                        "application/json")
            };

            return Task.FromResult(response);
        }
    }
}