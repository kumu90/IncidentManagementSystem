using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class SearchByDateDto
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string search { get; set; }
    }
}
