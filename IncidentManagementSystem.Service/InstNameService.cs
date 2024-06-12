using IncidentManagementSystem.DataAccess;
using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Service
{
    public class InstNameService : IInstNameService
    {
        readonly IInstNameDataAccess _iInstNameDataAccess;
        public InstNameService(IInstNameDataAccess iInstNameDataAccess)
        {
            _iInstNameDataAccess = iInstNameDataAccess;
        }

        public SQLStatusDto InstNameRegister(InstNameDto _instNameDto)
        {
            return _iInstNameDataAccess.InstNameRegister(_instNameDto);
        }

    }
    public interface IInstNameService
    {
        SQLStatusDto InstNameRegister(InstNameDto _instNameDto);
    }
}
