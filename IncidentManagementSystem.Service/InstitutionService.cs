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

        public List<InstNameDto> InstDetail()
        {
            return _iInstitutionDataAccess.InstDetail();
        }

        public List<InstNameDto> GetInstName()
        {
            return _iInstitutionDataAccess.GetInstName();
        }

        public List<Roles> RoleList()
        {
            return _iInstitutionDataAccess.RoleList();
        }

    }

    public interface IInstitutionService
    {
        SQLStatusDto InstNameRegister(InstNameDto _instNameDto);

        List<InstNameDto> InstDetail();

        List<InstNameDto> GetInstName();

        List<Roles> RoleList();
    }
}
