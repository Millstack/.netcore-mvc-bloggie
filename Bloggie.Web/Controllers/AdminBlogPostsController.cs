using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        public readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get all tags from repository
            var tags = await tagRepository.GetAllAsync();
            var model = new AddBlogRequest()
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString()})
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogRequest addBlogRequest)
        {
            // mapping view model to domain model
            var blogPost = new BlogPost()
            {
                Heading = addBlogRequest.Heading,
                PageTitle = addBlogRequest.PageTitle,
                Content = addBlogRequest.Content,
                ShortDescription = addBlogRequest.ShortDescription,
                FeaturedImageUrl = addBlogRequest.FeaturedImageUrl,
                UrlHandle = addBlogRequest.UrlHandle,
                PublishedDate = addBlogRequest.PublishedDate,
                Author = addBlogRequest.Author,
                Visible = addBlogRequest.Visible,
            };

            // mapping tags from only seletec tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = new Guid(selectedTagId);
                var existingTags = await tagRepository.GetAsync(selectedTagIdAsGuid);

                if(existingTags != null)
                {
                    selectedTags.Add(existingTags);
                }
            }

            // mapping selected tags back to domain model
            blogPost.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            // calling blogpost repo
            var blogPosts = await blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // retrive blogpost from repo
            var blogPostDomainModel = await blogPostRepository.GetAsync(id);

            var tagsDomainModel = await tagRepository.GetAllAsync();

            if (blogPostDomainModel != null)
            {
                // mapping domain model t view model
                var model = new EditBlogPostRequest()
                {
                    ID = blogPostDomainModel.ID,
                    Heading = blogPostDomainModel.Heading,
                    PageTitle = blogPostDomainModel.PageTitle,
                    Content = blogPostDomainModel.Content,
                    Author = blogPostDomainModel.Author,
                    FeaturedImageUrl = blogPostDomainModel.FeaturedImageUrl,
                    UrlHandle = blogPostDomainModel.UrlHandle,
                    ShortDescription = blogPostDomainModel.ShortDescription,
                    PublishedDate = blogPostDomainModel.PublishedDate,
                    Visible = blogPostDomainModel.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() }),
                    SelectedTags = blogPostDomainModel.Tags.Select(x => x.ID.ToString()).ToArray(),
                };
                return View(model);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // binding view model with domain model
            var blogPostDomainModel = new BlogPost()
            {
                ID = editBlogPostRequest.ID,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Visible = editBlogPostRequest.Visible,
                
            };

            // mapping tags into domain model
            var selectedTags = new List<Tag>();
            foreach(var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if(Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if(foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainModel.Tags = selectedTags;

            // submitting blogpost model to repo
            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

            if(updatedBlog != null)
            {
                // show success notification
                return RedirectToAction("Edit");
            }

            // show error notification
            return View("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.ID);

            if(deletedBlogPost != null)
            {
                // show success notification
                return RedirectToAction("List");
            }
            // show error notification
            return RedirectToAction("Edit", new { id = editBlogPostRequest.ID});
        }
    }
}
