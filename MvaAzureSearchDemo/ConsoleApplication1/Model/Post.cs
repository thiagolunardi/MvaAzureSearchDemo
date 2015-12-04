using System;
using Microsoft.Azure.Search.Models;

namespace MvaAzureSearchDemo.Model
{
    [SerializePropertyNamesAsCamelCase]
    public class Post
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Guid { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}