using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingProject.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    #region LoginViewModel Class

    /// <summary>
    /// Class having all types of fields with data annotations
    /// </summary>
    public class LoginViewModel
    {
        #region Properties
        /// <summary>
        /// Property for username field
        /// </summary>
        [Required(ErrorMessage = "User Name Is Required")]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        /// <summary>
        /// Property for password field
        /// </summary>
        [Required(ErrorMessage = "Password Is Required")]
        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Password Must Be Between 1-20 Characters")]
        public string Password { get; set; }

        /// <summary>
        /// Property for Department field
        /// </summary>
        [Required(ErrorMessage = "Domain Is Required")]
        [Display(Name = "Domain")]
        public byte Department { get; set; }

        /// <summary>
        /// Property for Remember field field
        /// </summary>
        public bool Remember { get; set; }
        #endregion
    }

    #endregion

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
    
}
