using System.Net;
using Bloggie.Web.Repositories;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        /*[HttpGet]
		public IActionResult Index()
		{
			return Ok("this is get image api call");
		}*/
        [HttpPost]
		public async Task<IActionResult>UploadAsync(IFormFile file) 
		{
			// call a repositry
			var imageURL=await imageRepository.UploadAsync(file);
			if(imageURL == null)
			{
				return Problem("Some thinmg went wrong!", null, (int)HttpStatusCode.InternalServerError);
			}
			return new JsonResult(new { Link = imageURL });
		}
	}
}
