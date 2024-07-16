using Bloggie.Web.Models.Domain;
namespace Bloggie.Web.Repositories

{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery=null,
            string? sortBy=null,
            string? sortDirection=null,
            int pageNumber=1,
            int pageSize=100);
        Task<Tag?> UpdateAsync(Tag tag);
        Task<Tag?> GetAsync(Guid id);
        Task<Tag> DeleteAsync(Guid id);
        Task<Tag> AddAsync(Tag tag);
        Task<int> CountAsync();
  
    }
}
