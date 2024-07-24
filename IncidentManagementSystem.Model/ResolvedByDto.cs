using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class ResolvedByDto
    {
        public string TicketId {  get; set; }
        public string AssignTo { get; set; }
        public string Resolve { get; set; }

    }
}
