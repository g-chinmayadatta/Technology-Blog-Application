using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Bloggie.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository) // use ITagRepository for the consuming trh methods in it
        {
            this.tagRepository = tagRepository;
        }
       
        [HttpGet]
        public IActionResult Add() // show form
        {
            return View();
        }
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest) // perform add operation
        {
            ValidateAddTagRequest(addTagRequest);
            if(ModelState.IsValid == false)
            {
                return View();
            }
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };
            await tagRepository.AddAsync(tag);
            return RedirectToAction("List");
        }

        private void ValidateAddTagRequest(AddTagRequest request)
        {
            if(request.Name is not null && request.DisplayName is not null)
            {
                if(request.Name == request.DisplayName)
                {
                    // raise the error
                    ModelState.AddModelError("DisplayName", "Name can't be same as DisplayName");
                }
            }
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List(
            string? searchQuery, 
            string? sortBy, 
            string? sortDirection,
            int pageSize=3,
            int pageNumber=1) // to display tags
        {
            
            var totalRecords=await tagRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);
            if (pageNumber > totalPages)
            {
                pageNumber--;
            }

            if (pageNumber < 1)
            {
                pageNumber++;
            }

            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.TotalPages=totalPages;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize=pageSize;
            // get tags form Db
            var tags =await tagRepository.GetAllAsync(searchQuery,sortBy,sortDirection,pageNumber,pageSize);
            return View(tags);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)  // must match with the route id name
        {
            // one wat to get th tag is 
            // var tag=await bloggieDbContext.Tags.FindAsync(id);
            var existingtag =await tagRepository.GetAsync(id);
            if(existingtag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = existingtag.Id,
                    Name = existingtag.Name,
                    DisplayName = existingtag.DisplayName,
                };
                return View(editTagRequest);

            }
            return View(null); 
        }
        [HttpPost]
        public async Task<IActionResult>Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };
            var updatedatg=await tagRepository.UpdateAsync(tag);
            if (updatedatg != null) {
                // show success notification 
                return RedirectToAction("List");
            }
            else
            {
                // error notification
            }
            return RedirectToAction("Edit", new { id=editTagRequest.Id});

        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest) 
        {
            var deletedtag = await tagRepository.DeleteAsync(editTagRequest.Id);
            if (deletedtag != null) {
                // show success notofication
                return RedirectToAction("List");    
            }
            // show error notifcation
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

    }
}
