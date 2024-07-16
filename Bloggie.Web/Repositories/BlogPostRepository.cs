using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog=await bloggieDbContext.BlogPosts.FindAsync(id);
            if(existingBlog != null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return await bloggieDbContext.BlogPosts.Include(x=>x.Tags).ToListAsync();
             
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            //return await bloggieDbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id); // this will only give blogpost information
            // to get tags infor associated with the blogpost we need to use Include(x=>x.Tags)
            return await bloggieDbContext.BlogPosts.Include(x=> x.Tags).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingtag=await bloggieDbContext.BlogPosts.Include(x=>x.Tags)
                .FirstOrDefaultAsync(x=> x.Id==blogPost.Id);
            if (existingtag != null)
            {
                existingtag.Id = blogPost.Id;
                existingtag.Heading = blogPost.Heading;
                existingtag.PageTitle = blogPost.PageTitle;
                existingtag.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingtag.UrlHandle = blogPost.UrlHandle;
                existingtag.Author = blogPost.Author;
                existingtag.Content = blogPost.Content;
                existingtag.PublishedDate = blogPost.PublishedDate;
                existingtag.Visible = blogPost.Visible;
                existingtag.Tags = blogPost.Tags;
                await bloggieDbContext.SaveChangesAsync();
                return existingtag;
            }
            return null;
        }
    }
}
