using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManage;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManage,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManage = userManage;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> Register(RegisterViewModel registerViewModel) 
        {
            if (ModelState.IsValid)
            {
				var identityUser = new IdentityUser
				{
					UserName = registerViewModel.Username,
					Email = registerViewModel.Email,
				};

				var identityResult = await userManage.CreateAsync(identityUser, registerViewModel.Password);
				if (identityResult.Succeeded)
				{
					// assigh hus user to User role
					var roleIdentityResult = await userManage.AddToRoleAsync(identityUser, "User");
					if (roleIdentityResult.Succeeded)
					{
						// show success notification
						return RedirectToAction("Register");
					}
				}
			}
            
            // show error notification
            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl) // ReturnUrl attribute
        {
            var model = new LoginViewModel
            {
                ReturnUrl = ReturnUrl
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) 
            {
                return View();
            }
            /*
             sign in manager help us to sign-in the user
             */
            var signInResult=await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password,false,false);
            if(signInResult != null && signInResult.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl)) 
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            // show error notification
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
 