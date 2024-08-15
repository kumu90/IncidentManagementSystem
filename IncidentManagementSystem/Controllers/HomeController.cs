using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace IncidentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInstitutionService _iInstitutionService;
        private readonly IUserService _userService;
        private readonly ITicketService _iTicketService;
        private readonly IAdminDashboardService _iadminDashboardService;
        public HomeController(IUserService userService, IInstitutionService institutionService, ITicketService ticketService, IAdminDashboardService iadminDashboardService)
        {
            _iInstitutionService = institutionService;
            _userService = userService;
            _iTicketService = ticketService;
            _iadminDashboardService = iadminDashboardService;
        }

        public HomeController()
        {
        }

        public void Init()
        {
            string userId = User.Identity.GetUserId();
            List<UserInfo> UserList = _userService.UserList(userId);
            ViewBag.UserList = new SelectList(UserList, "Id", "UserName");

            List<InstNameDto> institution = _iInstitutionService.GetInstName(userId);
            ViewBag.Institution = new SelectList(institution, "InstId", "InstitutionName");

            List<TicketDto> ticket = _iadminDashboardService.GetTicketList(userId);
            ViewBag.ticket = new SelectList(ticket, " InstId","TicketId");

            List<TicketDto> Pandinglist = _iadminDashboardService.GetTicketPandingStatusList(userId);
            ViewBag.Pandinglist = new SelectList(Pandinglist, "TicketId");

            List<ResolvedByDto> ResolveList = _iadminDashboardService.GetResolveList(userId);
            ViewBag.ResolveList = new SelectList(ResolveList, "TicketId");

            List<TicketDto> Rejectlist = _iadminDashboardService.GetTicketRejectStatusList(userId);
            ViewBag.Rejectlist = new SelectList(Rejectlist, "TicketId");
        }


        public ActionResult Index()
        {
            Init();
            string userId = User.Identity.GetUserId();
            return View("Dashboard");
        }

        [HttpGet]
        public ActionResult Dashboard(string userId = "")
        {
            Init();

            userId = User.Identity.GetUserId();

            var userList = _userService.UserList(userId);
            ViewBag.totalUsers = userList.Count();

            var InstitutionList = _iInstitutionService.GetInstName(userId);
            ViewBag.TotalInstitution = InstitutionList.Count();

            var TicketList = _iadminDashboardService.GetTicketList(userId);
            ViewBag.TotalTicket =TicketList.Count();

            var Pandinglist = _iadminDashboardService.GetTicketPandingStatusList(userId);
            ViewBag.TotalPandinglist = Pandinglist.Count();

            var  ResolveList = _iadminDashboardService.GetResolveList(userId);
            ViewBag.TotalTicketResolve = ResolveList.Count();

            var Rejectlist = _iadminDashboardService.GetTicketRejectStatusList(userId);
            ViewBag.TicketReject = Rejectlist.Count();

            var dataPoints = new List<DataPoint>
            {

                new DataPoint("Pending", ViewBag.TotalPandinglist),
                new DataPoint("Resolved", ViewBag.TotalTicketResolve),
                new DataPoint("Rejected", ViewBag.TicketReject)
            };
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            var userDataPoint = new List<DataPointUser>
            {
                 new DataPointUser("Total User", ViewBag.totalUsers),
                new DataPointUser("System User", 3),
                new DataPointUser("Users", 2),
                new DataPointUser("Active User", 5),
                new DataPointUser("Inactive User", 0),
                new DataPointUser("Active Insutitution", 2),
                new DataPointUser("Inactive Insutitution", 0)
            };
             ViewBag.UserDataPoint = JsonConvert.SerializeObject(userDataPoint);

            var model = new AdminDashboardDto
            {
                TotalUsers = ViewBag.totalUsers,
                TotalInstitution = ViewBag.TotalInstitution,
                TotalTicket = ViewBag.TotalTicket,
                TotalPandinglist = ViewBag.TotalPandinglist,
                TotalTicketResolve = ViewBag.TotalTicketResolve,
                TotalTicketReject = ViewBag.TicketReject,
                DataPoint = ViewBag.DataPoints
                //DataPointUser = ViewBag.DataPointsUser
            };


            return View(model);
            
        }
        
        public ActionResult Contact()
        {
            return View();
        }

        
    }
}