using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetAsync(Guid id); // find the blogpost
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost); // find and do opetaion
        Task<BlogPost?> DeleteAsync(Guid id); // find and do opetaions
        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle); // to find the blog using urlHandle
        

    }
}
