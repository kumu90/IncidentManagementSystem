using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class TicketDto
    {
        [Key]
        public string TicketId { get; set; }

        public DateTime date { get; set; }
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
        public string ImageUrl { get; set; }
    }
}
