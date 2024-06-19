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

        public List<UserDto> UserDetail()
        {
            return _iUserdataAccess.UserDetail();
        }
    }

    public interface IUserService
    {
        List<UserDto> UserDetail();
    }
}
