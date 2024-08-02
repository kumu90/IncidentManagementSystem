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
        public DateTime date { get; set; }

        [DisplayName("Status")]
        public string status { get; set; }

        [DisplayName("Institution Name")]
        [Required]
        public string InstId { get; set; }

        [Required]
        [DisplayName("Service")]
        public string ServiceId { get; set; }

        [Required]
        [DisplayName("Issue")]
        public string IssueId { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Cell Number")]
        public string CellNumber { get; set; }

        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }


        [DisplayName("Upload Supporting Image")]
        [Image]
        public string ImageUrl { get; set; }

        public byte[] ImageData { get; set; }
        public string contentType { get; set; }
        public int TotalCount { get; set; }

        public SearchDto Search { get; set; }
    }

    public class SearchDto
    {
        public string SearchById {  get; set; }
        public string SearchByName { get; set; }
    }
}
