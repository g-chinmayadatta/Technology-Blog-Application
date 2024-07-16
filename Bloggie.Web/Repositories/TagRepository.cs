using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext bloggieDbContext;
        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<int> CountAsync()
        {
            return await bloggieDbContext.Tags.CountAsync();
        }

        public  async Task<Tag> DeleteAsync(Guid id)
        {
           var existingtag= await bloggieDbContext.Tags.FindAsync(id);
            if (existingtag != null)
            {
                bloggieDbContext.Tags.Remove(existingtag);
                await bloggieDbContext.SaveChangesAsync();
                return existingtag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageNumber=1,
            int pageSize=100)
        {
            var query =bloggieDbContext.Tags.AsQueryable();
            // filtering
            if(string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.Name.Contains(searchQuery) || 
                                         x.DisplayName.Contains(searchQuery));
            }
            // sorting 
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);
                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query=isDesc? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }
                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);
                }
                
            }
            // pagination
            // ship 0 i.e; 1 to 5
            // skip 5 i.e; 5 to next 5 => 10
            var skipResults=(pageNumber-1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);
            return await query.ToListAsync();
            //return await bloggieDbContext.Tags.ToListAsync();
        }

        

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id==id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
           var existingtag= await bloggieDbContext.Tags.FindAsync(tag.Id);
            if (existingtag != null) 
            {
                existingtag.Name= tag.Name;
                existingtag.DisplayName= tag.DisplayName;
                await bloggieDbContext.SaveChangesAsync();
                return existingtag;

            }
            return null;
        }

    }
}
