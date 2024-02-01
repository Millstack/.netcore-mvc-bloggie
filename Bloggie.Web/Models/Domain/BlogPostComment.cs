namespace Bloggie.Web.Models.Domain
{
    public class BlogPostComment
    {
        public Guid ID { get; set; }
        public string Description { get; set; }
        public Guid BlogPostID { get; set; }
        public Guid UserID { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
