using Azure;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Bloggie.Web.Repository
{
    public class TagRepository : ITagRepository
    {
        // constructor injection
        private readonly BloggieDbContext bloggieDbContext;

        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        // add tag
        public async Task<Tag> AddAsync(Tag tag)
        {
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();
            return tag;
        }

        // get all tags
        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            // using bd context to read all the tags
            return await bloggieDbContext.Tags.ToListAsync();
        }

        // get single tag
        public Task<Tag?> GetAsync(Guid id)
        {
            return bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.ID == id);
        }

        // update tag
        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.ID);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // updating existing tag
                await bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;
        }

        // delete tag
        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(id);

            if(existingTag != null)
            {
                bloggieDbContext.Tags.Remove(existingTag);
                await bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }
    }
}
