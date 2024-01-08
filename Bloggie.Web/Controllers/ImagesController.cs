using Bloggie.Web.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("This is a GET image API call");
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            // calling repository to use the API
            var imageUrl = await imageRepository.UploadAsync(file);

            if(imageUrl != null) 
            {
                return new JsonResult(new { link = imageUrl });
            }
            else
            {
                return Problem("Something went wrong", null, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
