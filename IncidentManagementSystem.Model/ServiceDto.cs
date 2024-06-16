using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class ServiceDto
    {
        public string InstitutionName { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string status {  get; set; }
        public string Email {  get; set; }
        public string ContactNo { get; set; }
    }
}
