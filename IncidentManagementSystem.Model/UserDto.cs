using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class UserDto
    {
        public string Id { get; set; }
        
      
    }

    public class UserInfo
    {
        public string Id { get; set; }

        [DisplayName("User Name")]
        public string Username { get; set; }
        //public string UserName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Institution Name")]
        public string InstId { get; set; }

        [DisplayName("Role")]
        public string Roles { get; set; }

        public int TotalCount { get; set; }

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

    }

    public class UserListDto
    {
        public UserListDto()
        {
            UserList = new List<UserInfo>();
        }

        public List<UserInfo> UserList { get; set; }
        public int TotalCount { get; set; }
    }

}
