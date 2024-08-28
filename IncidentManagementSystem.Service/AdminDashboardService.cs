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
        public List<TicketDetailByMonthDto> GetMonthlyTicketDetails(string userId)
        {
            return _iAdminDashboadDataAccess.GetMonthlyTicketDetails(userId);
        }
        public List<DataPointServicesBase> GetServiceTicketCounts(string userId)
        {
            return _iAdminDashboadDataAccess.GetServiceTicketCounts(userId);
        }
        public List<DataPointInstitutionBase> GetInstitutionTicketCounts(string userId)
        {
            return _iAdminDashboadDataAccess.GetInstitutionTicketCounts(userId);
        }
        public List<TicketDto> GetRecentTicketActivityLog(string userId)
        {
            return _iAdminDashboadDataAccess.GetRecentTicketActivityLog(userId);
        }
        public List<TicketDto> GetRecentTicketStatusActivityLog(string userId)
        {
            return _iAdminDashboadDataAccess.GetRecentTicketStatusActivityLog(userId);
        }
        public List<TicketDto> GetRecentTicketStatusActivityLogUser(string userId)
        {
            return _iAdminDashboadDataAccess.GetRecentTicketStatusActivityLogUser(userId);
        }
    }
}

public interface IAdminDashboardService
{
    List<TicketDto> GetTicketList(string userId);
    List<TicketDto> GetTicketPandingStatusList(string userId);
    List<ResolvedByDto> GetResolveList(string userId);
    List<TicketDto> GetTicketRejectStatusList(string userId);
    List<TicketDetailByMonthDto> GetMonthlyTicketDetails(string userId);
    List<DataPointServicesBase> GetServiceTicketCounts(string userId);
    List<DataPointInstitutionBase> GetInstitutionTicketCounts(string userId);
    List<TicketDto> GetRecentTicketActivityLog(string userId);
    List<TicketDto> GetRecentTicketStatusActivityLog(string userId);
    List<TicketDto> GetRecentTicketStatusActivityLogUser(string userId);
}
