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
        private readonly IServiceInstutionDataAccess _iserviceInstutionDataAccess;

        public  ServiceInstutionService(IServiceInstutionDataAccess serviceInstutionDataAccess)
        {
            _iserviceInstutionDataAccess = serviceInstutionDataAccess;
        }

        public SQLStatusDto AddService(ServiceDto service)
        {
            return _iserviceInstutionDataAccess.AddService(service);
        }
        public SQLStatusDto TicketCreate(TicketDto _ticketDto)
        {
            return _iserviceInstutionDataAccess.TicketCreate(_ticketDto);
        }

        public List<ServiceDto> GetServiceName(string InstId = "")
        {
            return _iserviceInstutionDataAccess.GetServiceName(InstId);
        }

        //public SQLStatusDto RegisterService(RegisterServiceDto _registerServiceDto)
        //{
        //    return _serviceInstutionDataAccess.RegisterService(_registerServiceDto);
        //}
    }

    public interface IServiceInstutionService
    {
        SQLStatusDto AddService(ServiceDto service);
        SQLStatusDto TicketCreate(TicketDto ticketDto);
        List<ServiceDto> GetServiceName(string InstId = "");

        //SQLStatusDto RegisterService(RegisterServiceDto _registerServiceDto);
    }
}
