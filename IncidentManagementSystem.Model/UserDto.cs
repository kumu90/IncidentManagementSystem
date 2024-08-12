using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Institution Name")]
        public string InstId { get; set; }

        [DisplayName("Role")]
        public string Roles { get; set; }

        public int TotalCount { get; set; }

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
