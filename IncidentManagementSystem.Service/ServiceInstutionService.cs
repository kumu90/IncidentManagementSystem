using IncidentManagementSystem.DataAccess;
using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Service
{
    public class ServiceInstutionService : IServiceInstutionService
    {
        private readonly IServiceInstutionDataAccess _serviceInstutionDataAccess;

        public  ServiceInstutionService(IServiceInstutionDataAccess serviceInstutionDataAccess)
        {
            _serviceInstutionDataAccess = serviceInstutionDataAccess;
        }

        public SQLStatusDto AddService(ServiceDto service)
        {
            return _serviceInstutionDataAccess.AddService(service);
        }

        //public SQLStatusDto RegisterService(RegisterServiceDto _registerServiceDto)
        //{
        //    return _serviceInstutionDataAccess.RegisterService(_registerServiceDto);
        //}
    }

    public interface IServiceInstutionService
    {
        SQLStatusDto AddService(ServiceDto service);

        //SQLStatusDto RegisterService(RegisterServiceDto _registerServiceDto);
    }
}
