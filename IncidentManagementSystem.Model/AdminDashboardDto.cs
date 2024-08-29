using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int TotalInstitution{ get; set; }
        
        public int TotalTicket { get; set; }
        public int TotalPandinglist { get; set; }
       public int TotalTicketResolve { get; set; }
        public int TotalTicketReject { get; set; }
        public string DataPoint {  get; set; }
        public string TicketDetailByMonth { get; set; }
        public string DataServicesBase { get; set; }
        public string DataInstitutionBase {  get; set; }

        public string status { get; set; }




    }
}
