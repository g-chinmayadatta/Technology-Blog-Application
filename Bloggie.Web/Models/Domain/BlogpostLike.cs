namespace Bloggie.Web.Models.Domain
{
	public class BlogpostLike
	{
        public Guid Id { get; set; }
		public Guid BlogPostId { get; set; }
		public Guid UserId { get; set; }
    }
}
