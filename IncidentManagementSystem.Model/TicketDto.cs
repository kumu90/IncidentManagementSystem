using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncidentManagementSystem.Model.Annotation;

namespace IncidentManagementSystem.Model
{
    public class TicketDto
    {
        [Key]
        [DisplayName("Ticket Id")]
        public string TicketId { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }

        [DisplayName("Status")]
        public string status { get; set; }

        [DisplayName("Institution Name")]
        ///[Required]
        public string InstId { get; set; }
        public string InstitutionName {  get; set; }

       
        [DisplayName("Service")]
        [Required(ErrorMessage = "Please select service.")]
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }

        [Required]
        [DisplayName("Issue")]
        public string IssueId { get; set; }

     
        [DisplayName("Description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        [DisplayName("Cell Number")]
        public string CellNumber { get; set; }

        [Required]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


        [DisplayName("Upload Supporting Image")]
        [Image]
        public string ImageUrl { get; set; }

        public byte[] ImageData { get; set; }
        public string contentType { get; set; }
        public string userId { get; set; }
        public string UserName { get; set; }
        public string AssignedUsername { get; set; }
        public string RoleId { get; set; }
        public string AssineList {  get; set; }

    }

    public class SearchDto
    {
        public SearchDto() {
        ticketDtos = new List<TicketDto>();
        }

        public List<TicketDto> ticketDtos { get; set; }
        public int TotalCount { get; set; }
    }
}
