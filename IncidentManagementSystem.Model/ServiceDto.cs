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

    public class TicketDto
    {
        public string InstitutionName { get; set; }
        public string ServiceName { get; set; }
        public string Email { get; set; }

        [DisplayName("Contact No.")]
        public string ContectNo { get; set; }
        [DisplayName("Upload Supporting Image")]
        public string Image { get; set; }
    }
}
