﻿using IncidentManagementSystem.DataAccess;
using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
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
        public SQLStatusDto InstitutionCreate(InstNameDto _instNameDto)
        {
            return _iInstitutionDataAccess.InstitutionCreate(_instNameDto);
        }

        public InstListDto InstitutionList(string search, int page = 1, int offset = 10)
        {
            return _iInstitutionDataAccess.InstitutionList(search, page,offset);
        }

        public List<InstNameDto> GetInstName(string userId)
        {
            return _iInstitutionDataAccess.GetInstName(userId);
        }

        public List<Roles> RoleList()
        {
            return _iInstitutionDataAccess.RoleList();
        }

        public List<UserList> UserNameList()
        {
            return _iInstitutionDataAccess.UserNameList();
        }

        public Roles Rolename(string userId)
        {
            return _iInstitutionDataAccess.Rolename(userId);
        }
    }

    public interface IInstitutionService
    {
        SQLStatusDto InstitutionCreate(InstNameDto _instNameDto);

        InstListDto InstitutionList(string search, int page = 1, int offset = 10);

        List<InstNameDto> GetInstName(string userId);

        List<Roles> RoleList();
        List<UserList> UserNameList();
        Roles Rolename(string userId);
    }
}