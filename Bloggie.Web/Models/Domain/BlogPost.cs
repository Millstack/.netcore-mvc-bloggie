namespace Bloggie.Web.Models.Domain
{
    public class BlogPost
    {
        public Guid ID { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        // one to many
        // Navigation property
        public ICollection<Tag> Tags { get; set; }

        // one to many
        public ICollection<BlogPostLike> Likes { get; set; }

        // one blog post has multiple comments
        public ICollection<BlogPostComment> blogPostComment { get; set; }
    }
}
