using IncidentManagementSystem.DataAccess;
using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.Service
{
    public class TicketService : ITicketService
    {
        private readonly ITicketDataAccess _iTicketDataAccess;

        public TicketService(ITicketDataAccess iticketDataAccess)
        {
            _iTicketDataAccess = iticketDataAccess;
        }
        public SQLStatusDto TicketCreate(TicketDto _ticketDto)
        {
            return _iTicketDataAccess.TicketCreate(_ticketDto);
        }
        public List<TicketDto> TicketInfo(string search = "", string InstId = "",string status="",int page=1,int offset=10)
        {
            return _iTicketDataAccess.TicketInfo(search,InstId,status,page,offset);
        }
        public TicketDto GetTicketDetails(string TicketId)
        {
            return _iTicketDataAccess.GetTicketDetails(TicketId);
        }

        public List<IssueDto> GetIssueList(string ServiceId = "")
        {
            return _iTicketDataAccess.GetIssuesList(ServiceId);
        } 
        public TicketAssignDto TicketAssign(string TicketId)
        {
            return _iTicketDataAccess.TicketAssign(TicketId);
        }

        public TicketDto GetInstDetail(string UserName = "")
        {
            return _iTicketDataAccess.GetInstDetail(UserName);
        }

        public SQLStatusDto TicketAssignTo(TicketAssignDto AssignDto)
        {
            return _iTicketDataAccess.TicketAssignTo(AssignDto);
        }

        public SQLStatusDto TicketReject(string TicketId)
        {
            return _iTicketDataAccess.TicketReject(TicketId);
        }

        public ResolvedByDto GetResolveDetails(string TicketId)
        {
            return _iTicketDataAccess.GetResolveDetails(TicketId);
        }

        public SQLStatusDto TicketResolveBy(ResolvedByDto resolvedByDto)
        {
            return _iTicketDataAccess.TicketResolveBy(resolvedByDto);
        }
    }
}
public interface ITicketService
{
    SQLStatusDto TicketCreate(TicketDto ticketDto);
    List<TicketDto> TicketInfo(string search = "", string InstId = "", string status="", int page = 1, int offset = 10);

    TicketDto GetTicketDetails(string TicketId);
    List<IssueDto> GetIssueList(string ServiceId="");
    TicketAssignDto TicketAssign(string TicketId = "");
    SQLStatusDto TicketAssignTo(TicketAssignDto AssignDto);
    SQLStatusDto TicketReject(string TicketId);

    TicketDto GetInstDetail(string UserName = "");
    ResolvedByDto GetResolveDetails(string TicketId);
    SQLStatusDto TicketResolveBy(ResolvedByDto resolvedByDto);

}