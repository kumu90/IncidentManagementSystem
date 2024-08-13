using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Model
{
    public class ErrorLogDto
    {
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string userId { get; set; }
        public string Message { get; set; }
    }
}
