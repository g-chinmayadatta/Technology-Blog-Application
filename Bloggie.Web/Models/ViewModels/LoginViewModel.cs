﻿using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public   string Username { get; set; }
        [Required]
        [MinLength(6,ErrorMessage ="Password has to be 6 charaters")]
        public string Password { get; set; }
        
        public string? ReturnUrl { get; set; }
    }
}
