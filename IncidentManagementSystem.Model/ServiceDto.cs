using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IncidentManagementSystem.Model
{
    public class ServiceDto
    {
        [DisplayName ("Service Name")]
        public string ServiceName { get; set; }
        public string Institution { get; set; }
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

        [DisplayName("Institution Name")]
        public string InstId { get; set; }

        [DisplayName("Service")]
        public string ServiceId { get; set; }

        [DisplayName("Description")]
        public string Description {  get; set; }

        [DisplayName("Contact")]
        public string ContectNo { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        
        [DisplayName("Upload Supporting Image")]
        public string Image { get; set; }
    }
}
