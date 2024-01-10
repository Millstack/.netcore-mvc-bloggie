using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            // wil receive the urlhandle, will use to retrive the data
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

            return View(blogPost);
        }
    }
}
