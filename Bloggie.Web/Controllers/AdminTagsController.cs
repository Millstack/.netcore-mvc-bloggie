using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;
        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            // incoming request
            //string name = Request.Form["name"];

            // model binding
            //var name = addTagRequest.Name;

            var tag = new Tag()
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            // using bd context to read all the tags
            var tags = await bloggieDbContext.Tags.ToListAsync();

            return View(tags);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            // name of i/p parameter must be same s asp-route-id tag helper

            // 1st method
            //bloggieDbContext.Tags.Find(id);

            // 2nd method
            var tag = await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.ID == id);

            if(tag != null) 
            {
                var editTagRequest = new EditTagRequest()
                {
                    ID = tag.ID,
                    Name = tag.Name,  
                    DisplayName = tag.DisplayName,
                };
                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag()
            {
                ID = editTagRequest.ID,
                Name  = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };

            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.ID);

            if(existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // updating existing tag
                await bloggieDbContext.SaveChangesAsync();

                // showing sucess notification
                return RedirectToAction("Edit", new { id = editTagRequest.ID });
            }

            // showing failure notification
            return RedirectToAction("Edit", new { id = editTagRequest.ID });
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var tag = await bloggieDbContext.Tags.FindAsync(editTagRequest.ID);

            if(tag != null )
            {
                bloggieDbContext.Tags.Remove(tag);
                await bloggieDbContext.SaveChangesAsync();

                // show success notification
                return RedirectToAction("List");
            }

            // showing error nitification
            return RedirectToAction("Edit", new { id = editTagRequest.ID });
        }
    }
}
