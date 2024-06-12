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
        public List<InstNameDto> GetInstName(InstNameDto _instNameDto)
        {
            return _getInstNameDataAccess.GetInstName(_instNameDto);
        }
    }
    public interface IGetInstNameService
    {
        List<InstNameDto> GetInstName(InstNameDto instNameDto);
    }
}
