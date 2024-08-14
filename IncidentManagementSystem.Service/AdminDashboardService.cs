using IncidentManagementSystem.DataAccess;
using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Service
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboadDataAccess _iAdminDashboadDataAccess;

        public AdminDashboardService(IAdminDashboadDataAccess iAdminDashboadDataAccess)
        {
            _iAdminDashboadDataAccess = iAdminDashboadDataAccess;
        }
        public List<TicketDto> GetTicketList(string userId)
        {
            return _iAdminDashboadDataAccess.GetTicketList(userId);
        }
        public List<TicketDto> GetTicketPandingStatusList(string userId)
        {
            return _iAdminDashboadDataAccess.GetTicketPandingStatusList(userId);

        }
        public List<ResolvedByDto> GetResolveList(string userId)
        {
            return _iAdminDashboadDataAccess.GetResolveList(userId);
        }
        public List<TicketDto> GetTicketRejectStatusList(string userId)
        {
            return _iAdminDashboadDataAccess.GetTicketRejectStatusList(userId);

        }
    }
}

public interface IAdminDashboardService
{
    List<TicketDto> GetTicketList(string userId);
    List<TicketDto> GetTicketPandingStatusList(string userId);
    List<ResolvedByDto> GetResolveList(string userId);
    List<TicketDto> GetTicketRejectStatusList(string userId);
}
