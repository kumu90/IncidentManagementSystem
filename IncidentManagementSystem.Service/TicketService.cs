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
        public List<TicketDto> TicketInfo(string search = "")
        {
            return _iTicketDataAccess.TicketInfo(search);
        }
        public TicketDto GetTicketDetails(string TicketId)
        {
            return _iTicketDataAccess.GetTicketDetails(TicketId);
        }

        public List<IssueDto> GetIssueList()
        {
            return _iTicketDataAccess.GetIssuesList();
        } 
    }
}
public interface ITicketService
{
    SQLStatusDto TicketCreate(TicketDto ticketDto);
    List<TicketDto> TicketInfo(string search = "");

    TicketDto GetTicketDetails(string TicketId);
    List<IssueDto> GetIssueList();
}