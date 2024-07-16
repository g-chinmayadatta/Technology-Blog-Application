using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComments> AddAsync(BlogPostComments blogPostComments);
        Task<IEnumerable<BlogPostComments>> GetCommentByBlogId(Guid blogPosId);
    }
}
