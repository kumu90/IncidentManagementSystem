using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IncidentManagementSystem.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }
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

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }
    }

    public class LoginViewModel
    {
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        [Display(Name = "UserName")]
        //[EmailAddress]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {

        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Institution Name")]
        [Required]
        public string InsId { get; set; }

        [Display(Name = "Roles")]
        [Required]
        public string UserRole_Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
        ErrorMessage = "Password must be at least 6 characters long and include one uppercase letter, one lowercase letter, one number, and one special character.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        //[Required]
        //public string Name { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "User Name")]
        public string Username { get; set; }

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

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "User Name")]
        public string Username { get; set; }
    }

    /// custom model
    public class EditProfileViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        [Display(Name = "Roles")]
        public string UserRole_Id { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
        ErrorMessage = "Password must be at least 6 characters long and include one uppercase letter, one lowercase letter, one number, and one special character.")]
        [Display(Name = "New Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public bool ShowPartialView { get; set; }
        public bool UpdateUserName { get; set; }
        [Display(Name = "New UserName")]
        public string NewUserName {  get; set; }
        [Display(Name = "New Role")]
        public string UserRoleId {  get; set; }

        
        //public string InstitutionName { get; set; }
        //public string Email { get; set; }
        //public string Country { get; set; } 
        //public string State { get; set; }
        //public string ZipCode { get; set; }
        //public string ContactNumber { get; set; }

    }
}
