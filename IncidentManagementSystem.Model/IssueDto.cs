using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class IssueDto
    {
        public int IssueId { get; set; }
        public string IssueName { get; set; }
        public int ServiceId { get; set; }
        public string Description { get; set; }
    }
}
