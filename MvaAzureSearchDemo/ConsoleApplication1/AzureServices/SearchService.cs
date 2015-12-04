using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using MvaAzureSearchDemo.Model;

namespace MvaAzureSearchDemo.AzureServices
{
    public class SearchService : IDisposable
    {
        private readonly ISearchServiceClient _searchServiceClient;
        private const string IndexName = "posts";

        public SearchService(string searchServiceName, string apiKey)
        {
            _searchServiceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            CreateOrUpdateIndex();
        }

        public int UploadDocumentsToIndex(IEnumerable<Post> posts)
        {
            var indexClient = _searchServiceClient.Indexes.GetClient(IndexName);
            var docResponse = indexClient.Documents.Index(IndexBatch.Create(posts.Select(IndexAction.Create)));
            return docResponse.Results.Count;
        }

        private void CreateOrUpdateIndex()
        {
            #region Suggester
            var suggester = new Suggester
            {
                Name = "PostSuggester",
                SearchMode = SuggesterSearchMode.AnalyzingInfixMatching,
                SourceFields = new[] {"title", "category"}
            };
            #endregion

            var definition = new Index
            {
                Name = IndexName,
                Fields = new[]
                {
                    new Field("id", DataType.String) {IsKey = true},
                    new Field("date", DataType.DateTimeOffset) {IsSortable = true},
                    new Field("title", DataType.String) {IsSearchable = true, IsSortable = true},
                    new Field("content", DataType.String) {IsSearchable = true, IsRetrievable = false},
                    new Field("guid", DataType.String),
                    new Field("category", DataType.String) {IsSearchable = true, IsFacetable = true, IsFilterable = true},
                    new Field("description", DataType.String) {IsSearchable = true}
                },
                Suggesters = new[] {suggester}
            };

            if (_searchServiceClient.Indexes.Exists(IndexName))
                _searchServiceClient.Indexes.Delete(IndexName);

            _searchServiceClient.Indexes.Create(definition);
        }

        public void Dispose()
        {
            _searchServiceClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}