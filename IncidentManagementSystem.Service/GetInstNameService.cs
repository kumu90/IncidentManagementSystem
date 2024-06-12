using IncidentManagementSystem.DataAccess;
using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Service
{
    public class GetInstNameService : IGetInstNameService
    {
        readonly IGetInstNameDataAccess _getInstNameDataAccess;
        public GetInstNameService(IGetInstNameDataAccess getInstNameDataAccess) 
        {
            _getInstNameDataAccess = getInstNameDataAccess;
        }
        public List<InstNameDto> GetInstName()
        {
            return _getInstNameDataAccess.GetInstName();
        }
    }
    public interface IGetInstNameService
    {
        List<InstNameDto> GetInstName();
    }
}
