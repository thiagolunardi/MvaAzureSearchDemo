using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Search.Models;

namespace AzureSearchWebApiDemo.Models
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Guid { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double Score { get; set; }

        public static IList<Post> ToModels(IList<SearchResult<Post>> results)
        {
            var posts = new List<Post>();

            for (int idx = 0; idx < results.Count; idx++)
            {
                var title = results[idx].Highlights?.SingleOrDefault(hl => hl.Key == "title").Key != null
                    ? results[idx].Highlights["title"][0]
                    : results[idx].Document.Title;

                var description = results[idx].Highlights?.SingleOrDefault(hl => hl.Key == "description").Key != null
                    ? results[idx].Highlights["description"][0]
                    : results[idx].Document.Description;

                posts.Add(new Post
                {
                    Id = results[idx].Document.Id,
                    Date = results[idx].Document.Date,
                    Title = title,
                    Guid = results[idx].Document.Guid,
                    Category = results[idx].Document.Category,
                    Description = description,
                    Score = results[idx].Score
                });
            }

            return posts;
        }
    }
}