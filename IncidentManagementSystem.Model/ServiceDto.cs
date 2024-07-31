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

        [DisplayName("Institute Name")]
        public string InstId { get; set; }
        public TicketDto ticket { get; set; }
    }
  
}
