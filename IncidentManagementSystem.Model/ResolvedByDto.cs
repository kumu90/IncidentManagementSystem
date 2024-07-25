using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class ResolvedByDto
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Username { get; set; }
        public string TicketId {  get; set; }
        public string IssueId { get; set; }
        public string InstId { get; set; }
        public string AssignTo { get; set; }
        public string Resolve { get; set; }
        public string Description {  get; set; }

    }
}
