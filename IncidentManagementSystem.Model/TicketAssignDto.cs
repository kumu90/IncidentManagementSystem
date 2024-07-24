using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class TicketAssignDto
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }

        [DisplayName("IssueId")]
        public string IssueId { get; set; }

        [DisplayName("ServiceId")]
        public string ServiceId { get; set; }

        [DisplayName("Assign")]
        public string AssignTo { get; set; }

        [DisplayName("TicketId")]
        public string TicketId { get; set; }
    }
}
