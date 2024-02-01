using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repository
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
        Task<IEnumerable<BlogPostComment>> GetCOmmentsByBlogId(Guid blogPostId);
    }
}
