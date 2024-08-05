using IncidentManagementSystem.DataAccess;
using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Service
{
    public class UserService:IUserService
    {
        private readonly IUserDataAccess _iUserdataAccess;
        public UserService(IUserDataAccess userDataAccess) 
        {
            _iUserdataAccess = userDataAccess;
        }

        public List<UserInfo> UserDetail(string search, int page = 1, int offset = 10)
        {
            return _iUserdataAccess.UserDetail(search,page,offset);
        }
    }

    public interface IUserService
    {
        List<UserInfo> UserDetail(string search, int page = 1, int offset = 10);
    }
}
