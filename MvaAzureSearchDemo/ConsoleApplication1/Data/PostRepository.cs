using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dapper;
using MvaAzureSearchDemo.Model;

namespace MvaAzureSearchDemo.Data
{
    public class PostRepository : MySqlContext
    {
        public IEnumerable<Post> All()
        {
            using (var cn = Connection)
            {
                var query = string.Concat(
                    "SELECT p.id, p.post_date DATE, p.post_content content, p.post_title title, p.guid, t.name category, pm.meta_value description",
                    "  FROM wp_posts p",
                    "  JOIN wp_term_relationships r ON r.object_id = p.id",
                    "  JOIN wp_term_taxonomy tx ON tx.term_taxonomy_id = r.term_taxonomy_id AND tx.taxonomy = 'category' AND (tx.parent > 0 OR tx.term_taxonomy_id = 122)",
                    "  JOIN wp_terms t ON t.term_id = tx.term_id",
                    "  JOIN wp_postmeta pm ON pm.post_id = p.id AND meta_key = '_yoast_wpseo_metadesc'",
                    " WHERE p.post_type = 'post'");

                var posts = cn.Query<Post>(query).ToList();

                var htmlTags = new Regex(@"<[^>]*(>|$)");
                var breaks = new Regex(@"[\s\r\n]");

                posts.ForEach(post =>
                {
                    post.Content = htmlTags.Replace(post.Content, string.Empty).Trim();
                    post.Content = breaks.Replace(post.Content, " ").Trim();
                });

                return posts;
            }
        }
    }
}