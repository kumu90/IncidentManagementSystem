using IncidentManagementSystem.DataAccess;
using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Service
{
    public class InstNameService : iInstNameService
    {
        readonly iInstNameDataAccess _iInstNameDataAccess;
        public InstNameService(iInstNameDataAccess iInstNameDataAccess)
        {
            _iInstNameDataAccess = iInstNameDataAccess;
        }

        public string InstNameRegister(InstNameDto _instNameDto)
        {
            return _iInstNameDataAccess.InstNameRegister(_instNameDto);
        }

    }
    public interface iInstNameService
    {
        string InstNameRegister(InstNameDto _instNameDto);
    }
}
