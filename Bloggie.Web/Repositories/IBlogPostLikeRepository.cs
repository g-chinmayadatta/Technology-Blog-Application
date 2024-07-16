using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
	public interface IBlogPostLikeRepository
	{
		Task<int> GetTotalLikes(Guid blogPostId);
		Task<IEnumerable<BlogpostLike>> GetLikesForBlog(Guid blogPostId);
		Task<BlogpostLike> AddLikeForBlog(BlogpostLike blogpostLike);
	}
}
