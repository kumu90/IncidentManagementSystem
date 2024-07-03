using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IncidentManagementSystem.Model
{
    public class ServiceDto
    {
        [DisplayName("Services")]
        public int ServiceId { get; set; }
                
        public string ServiceName { get; set; }
        public bool Flag { get; set; }
        public string InstId { get; set; }
    }
    public class RegisterServiceDto
    {

        public string Id { get; set; }

        [DisplayName("Service Name")]
        public string serviceName { get; set; }

        [DisplayName("Institution Name")]
        public string InstId { get; set; }
        
    }
    public class TicketDto
    {
        [Key]
        public int TicketId { get; set; }

        public DateTime date { get; set; }
        public bool status { get; set; }

        [DisplayName("Institution Name")]
        [Required]
        public string InstId { get; set; }

        [Required]
        [DisplayName("Service")]
        public string ServiceId { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description {  get; set; }

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
