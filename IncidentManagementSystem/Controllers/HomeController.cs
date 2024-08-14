using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using Microsoft.AspNet.Identity;
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
        private readonly IAdminDashboadDataAccess _iadminDashboadDataAccess;
        public HomeController(IUserService userService, IInstitutionService institutionService, ITicketService ticketService,IAdminDashboadDataAccess iadminDashboadDataAccess)
        {
            _iInstitutionService = institutionService;
            _userService = userService;
            _iTicketService = ticketService;
            _iadminDashboadDataAccess = iadminDashboadDataAccess;
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

            List<TicketDto> ticket = _iadminDashboadDataAccess.GetTicketList(userId);
            ViewBag.Institution = new SelectList(ticket, " InstId","TicketId");

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
            var totalUsers = userList.Count();

            var InstitutionList = _iInstitutionService.GetInstName(userId);
            var TotalInstitution = InstitutionList.Count();

            var TicketList = _iadminDashboadDataAccess.GetTicketList(userId);
            //var TotalTicket = TicketList;

            //var model = new AdminDashboardDto
            //{
            //    TotalUsers = totalUsers,
            //    TotalInstitution = TotalInstitution,
            //    TotalTicket = TicketList.Count()
            //};

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalInstitution = TotalInstitution;
            ViewBag.TotalTicket = TicketList.Count();

            return View();
            
        }
        
        public ActionResult Contact()
        {
            return View();
        }

        
    }
}