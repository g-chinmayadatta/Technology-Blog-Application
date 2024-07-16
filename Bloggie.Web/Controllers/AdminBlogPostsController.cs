using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepositry;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepositry,IBlogPostRepository blogPostRepository)
        {
            this.tagRepositry = tagRepositry;
            this.blogPostRepository = blogPostRepository;
        }
        [HttpGet]
       public async Task<IActionResult> Add()
        {
            // fetch all tags form db
            var tags=await tagRepositry.GetAllAsync();
            var model = new AddBlogPostRequest { 
                Tags=tags.Select(x => new SelectListItem { Text=x.DisplayName,Value=x.Id.ToString()}),
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {

            // map selected tags to tags
            var tempSelectedTags = new List<Tag>();
            foreach(var i in addBlogPostRequest.SelectedTags)
            {
                // select each tag id and extract it from db
                var selectedTagIdGuid = Guid.Parse(i);
                var existingtag = await tagRepositry.GetAsync(selectedTagIdGuid);
                if (existingtag != null) { 
                    tempSelectedTags.Add(existingtag);
                }

            }   
            // map viewmodel to domain model
            var blogpost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                Visible = addBlogPostRequest.Visible,
                Tags = tempSelectedTags,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author=addBlogPostRequest.Author,
            };
            await blogPostRepository.AddAsync(blogpost);
            return RedirectToAction("Add");
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogposts = await blogPostRepository.GetAllAsync(); // tags won't com ethis way 
            return View(blogposts); 
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // retrive that blog post from database
            var blogpost=await blogPostRepository.GetAsync(id);
            // mapping tags 
            var tagsDomainModel=await tagRepositry.GetAllAsync();
            // map the domain model to view model
            if (blogpost != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = blogpost.Id,
                    Heading = blogpost.Heading,
                    PageTitle = blogpost.PageTitle,
                    Content = blogpost.Content,
                    ShortDescription = blogpost.ShortDescription,
                    FeaturedImageUrl = blogpost.FeaturedImageUrl,
                    UrlHandle = blogpost.UrlHandle,
                    Author = blogpost.Author,
                    PublishedDate = blogpost.PublishedDate,
                    Visible = blogpost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogpost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
            return View(null);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // map tags into domain model
            var stags = new List<Tag>();
            foreach(var selectedtag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedtag, out var tag)) // since we are using tags id as value
                {
                    if (tag != null)
                    {
                        var foundTag = await tagRepositry.GetAsync(tag); // tag is guid 
                        stags.Add(foundTag);// get the tag form db and add it to the blog post
                    }
                } 
            }
            // viewmodel to domain model
            var blogPostDomainModel = new BlogPost
            {
                Id=editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible,
                Tags = stags,
            };
            // sumbit to repo
            var updatedblog=await blogPostRepository.UpdateAsync(blogPostDomainModel);
            if(updatedblog!=null)
            {
                // success message
                //Redirect to List method
                return RedirectToAction("Edit");
            }
            // show error message 
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest) 
        {
            // takl to repo and dlete the tags and blogpost
            var deletedblogpost=await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            // display the responce
            if (deletedblogpost != null)
            {
                //show success message 
                return RedirectToAction("List");
            }
            // show error message
            return RedirectToAction("Edit",new {id=editBlogPostRequest.Id });
           
        }
    }
}
