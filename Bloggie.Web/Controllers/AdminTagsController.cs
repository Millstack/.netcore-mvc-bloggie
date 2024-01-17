using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
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

            await tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            var tags = await tagRepository.GetAllAsync();

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
            var tag = await tagRepository.GetAsync(id);

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

            var updatedTag = await tagRepository.UpdateAsync(tag);

            if(updatedTag != null)
            {
                // show success notification
            }
            else
            {
                // show error noo=tification
            }

            // showing failure notification
            return RedirectToAction("Edit", new { id = editTagRequest.ID });
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.ID);

            if(deletedTag != null)
            {
                // showing success nitification
                return RedirectToAction("List");
            }

            // showing error nitification
            return RedirectToAction("Edit", new { id = editTagRequest.ID });
        }
    }
}
