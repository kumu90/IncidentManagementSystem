using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Service
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly IErrorLogDataAccess _iErrorLogDataAccess;
        public ErrorLogService(IErrorLogDataAccess errorLogDataAccess)
        {
            _iErrorLogDataAccess = errorLogDataAccess;
        }
        public void LogError(ErrorLogDto errorLog)
        {
            _iErrorLogDataAccess.LogError(errorLog);
        }
    }
}

public interface IErrorLogService
{
    void LogError(ErrorLogDto errorLog);
}
