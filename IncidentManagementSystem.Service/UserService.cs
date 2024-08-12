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

        public UserListDto UserDetail(string search, int page = 1, int offset = 10, string userId = "")
        {
            return _iUserdataAccess.UserDetail(search,page,offset, userId);

        }
        public List<UserInfo> UserList(string userId)
        {
            return _iUserdataAccess.UserList(userId);
        }
    }

    public interface IUserService
    {
        UserListDto UserDetail(string search, int page = 1, int offset = 10, string userId = "");
        List<UserInfo> UserList(string userId);
    }
}
