using IncidentManagementSystem.DataAccess;
using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Service
{
    public class InstitutionService: IInstitutionService
    {
        readonly IInstitutionDataAccess _iInstitutionDataAccess;
        public InstitutionService(IInstitutionDataAccess InstNameDataAccess)
        {
            _iInstitutionDataAccess = InstNameDataAccess;
        }
        public SQLStatusDto InstNameRegister(InstNameDto _instNameDto)
        {
            return _iInstitutionDataAccess.InstNameRegister(_instNameDto);
        }
        public List<InstNameDto> GetInstName()
        {
            return _iInstitutionDataAccess.GetInstName();
        }
    }

    public interface IInstitutionService
    {
        SQLStatusDto InstNameRegister(InstNameDto _instNameDto);
        List<InstNameDto> GetInstName();
    }
}
