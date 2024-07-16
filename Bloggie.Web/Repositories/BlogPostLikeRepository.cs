
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
	public class BlogPostLikeRepository : IBlogPostLikeRepository
	{
		private readonly BloggieDbContext bloggieDbContext;

		public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
        {
			this.bloggieDbContext = bloggieDbContext;
		}

        public async Task<BlogpostLike> AddLikeForBlog(BlogpostLike blogpostLike)
        {
			await bloggieDbContext.BlogpostLike.AddAsync(blogpostLike);
			await bloggieDbContext.SaveChangesAsync();
			return blogpostLike;
        }

        public async Task<IEnumerable<BlogpostLike>> GetLikesForBlog(Guid blogPostId)
        {
			return await bloggieDbContext.BlogpostLike
				.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
		{
			return await bloggieDbContext.BlogpostLike
				.CountAsync(x => x.BlogPostId == blogPostId);

		}
	}
}
