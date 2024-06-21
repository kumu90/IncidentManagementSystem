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
        public string InstId { get; set; }
        public string InstitutionName { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string ContactPersonAdmin { get; set; }
        public string ContactPersonTechnical { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string Flag { get; set; }
      
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

    }
}
