using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPostComments> AddAsync(BlogPostComments blogPostComments)
        {
            await bloggieDbContext.BlogPostComment.AddAsync(blogPostComments);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostComments;
        }

        public async Task<IEnumerable<BlogPostComments>> GetCommentByBlogId(Guid blogPosId)
        {
            return await bloggieDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPosId).ToListAsync();
        }
    }
}
