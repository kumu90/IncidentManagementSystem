using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class RegisterServiceDto
    {
        public string Id { get; set; }

        [DisplayName("Service Name")]
        public string serviceName { get; set; }

        [DisplayName("Institution Name")]
        public string InstId { get; set; }
    }
}
