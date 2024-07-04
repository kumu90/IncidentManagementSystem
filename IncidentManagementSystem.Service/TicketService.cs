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
        private readonly ITicketDataAccess _iticketDataAccess;

        public TicketService(ITicketDataAccess iticketDataAccess)
        {
            _iticketDataAccess = iticketDataAccess;
        }
        public SQLStatusDto TicketCreate(TicketDto _ticketDto)
        {
            return _iticketDataAccess.TicketCreate(_ticketDto);
        }
        public List<TicketDto> TicketInfo(string search = "")
        {
            return _iticketDataAccess.TicketInfo(search);
        }
        public TicketDto GetTicketDetails(int TicketId)
        {
            return _iticketDataAccess.GetTicketDetails(TicketId);
        }
    }
}
public interface ITicketService
{
    SQLStatusDto TicketCreate(TicketDto ticketDto);
    List<TicketDto> TicketInfo(string search = "");

    TicketDto GetTicketDetails(int TicketId);
}