using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bloggie.Web.Repository
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.AddAsync(blogPost);
            bloggieDbContext.SaveChanges();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog =  await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.ID ==  blogPost.ID);

            if(existingBlog != null)
            {
                existingBlog.ID = blogPost.ID;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.Tags = blogPost.Tags;

                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            else
            {
                return null;
            }
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await bloggieDbContext.BlogPosts.FindAsync(id);

            if(existingBlogPost != null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlogPost);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            else { return null; }
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);

        }
    }
}
