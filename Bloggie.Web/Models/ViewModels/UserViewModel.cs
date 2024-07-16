namespace Bloggie.Web.Models.ViewModels
{
	public class UserViewModel
	{
        public List<User> Users { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public bool AdminRoleCheckbox { get; set; }
    }
}
