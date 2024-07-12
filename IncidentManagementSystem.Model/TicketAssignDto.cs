using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class TicketAssignDto
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string IssueId { get; set; }
        public string Status { get; set; }
        public string AssignTo { get; set; }
        public string TicketId { get; set; }
    }
}
