using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blogpost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var model = new BlogDetailsViewModel();

            if (blogpost != null)
            {
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogpost.Id);
                if (signInManager.IsSignedIn(User))
                {
                    // get all likes for this blog for this user
                    var likesForBlog = await blogPostLikeRepository.GetLikesForBlog(blogpost.Id);
                    var userId = userManager.GetUserId(User);
                    if (userId != null)
                    {
                        var likeforuser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeforuser != null;
                    }
                }
                // get comment fo rthis blog
                var blogCommentsDomainModel   = await blogPostCommentRepository.GetCommentByBlogId(blogpost.Id);
                var blogCommentsForView = new List<BlogComment>();
                foreach (var comment in blogCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = comment.Description,
                        DateAdded = comment.DateAdded,
                        UserName = (await userManager.FindByIdAsync(comment.UserId.ToString())).UserName
                    });
                }
                model = new BlogDetailsViewModel
                {
                    Id = blogpost.Id,
                    Heading = blogpost.Heading,
                    PageTitle = blogpost.PageTitle,
                    Tags = blogpost.Tags,
                    UrlHandle = blogpost.UrlHandle,
                    FeaturedImageUrl = blogpost.FeaturedImageUrl,
                    Author = blogpost.Author,
                    PublishedDate = blogpost.PublishedDate,
                    Content = blogpost.Content,
                    ShortDescription = blogpost.ShortDescription,
                    Visible = blogpost.Visible,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = blogCommentsForView,
                    
                };
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var model = new BlogPostComments {
                     BlogPostId = blogDetailsViewModel.Id,
                     Description = blogDetailsViewModel.Comment,
                     DateAdded=DateTime.Now, 
                     UserId = Guid.Parse(userManager.GetUserId(User))
                };
                await blogPostCommentRepository.AddAsync(model);
                return RedirectToAction("Index", "Blogs",
                    new { urlHandle = blogDetailsViewModel.UrlHandle });
            }
            return View();
            
           
        }
    }
}
