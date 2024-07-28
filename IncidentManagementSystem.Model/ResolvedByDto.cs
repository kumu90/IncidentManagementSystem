using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class ResolvedByDto
    {
        public string Id { get; set; }

        [DisplayName("Transation Date")]
        public DateTime TranDateTime { get; set; }
        public DateTime date {  get; set; }

        [DisplayName("User Name")]
        public string Username { get; set; }

        [DisplayName("Ticket")]
        public string TicketId {  get; set; }

        [DisplayName("Issue")]
        public string IssueId { get; set; }

        [DisplayName("Institution Name")]
        public string InstId { get; set; }

        [DisplayName("AssignTo")]
        public string AssignTo { get; set; }

        [DisplayName("Resolve")]
        public string Resolve { get; set; }

        public string Description {  get; set; }


    }
}
