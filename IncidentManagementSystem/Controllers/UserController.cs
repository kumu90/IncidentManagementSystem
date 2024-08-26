using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using Microsoft.Ajax.Utilities;
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
    [Authorize]
    public class UserController : Controller
    {
        ///private readonly IInstitutionService _iInstitutionService;
        private readonly IUserService _userService;
        private readonly IAdminDashboardService _iadminDashboardService;

        public UserController()
        {

        }

        public UserController( IUserService userService, IAdminDashboardService iadminDashboardService)
        {
            ///_iInstitutionService = iInstitutionService;
            _userService = userService;
            _iadminDashboardService = iadminDashboardService;
        }

        public void Init()
        {
            string userId = User.Identity.GetUserId();
            List<UserInfo> UserList = _userService.UserList(userId);
            ViewBag.UserList = new SelectList(UserList, "Id", "UserName");

            List<TicketDto> Pandinglist = _iadminDashboardService.GetTicketPandingStatusList(userId);
            ViewBag.Pandinglist = new SelectList(Pandinglist, "TicketId");

            List<ResolvedByDto> ResolveList = _iadminDashboardService.GetResolveList(userId);
            ViewBag.ResolveList = new SelectList(ResolveList, "TicketId");

            List<TicketDto> Rejectlist = _iadminDashboardService.GetTicketRejectStatusList(userId);
            ViewBag.Rejectlist = new SelectList(Rejectlist, "TicketId");
        }



        [Authorize(Roles = "SuperAdmin, Admin, Developer")]
        public ActionResult Index(/*string search*/)
        {
            Init();
            //var clients = _userService.UserDetail(search);
            //return View(clients);
            return View();
        }

        [Authorize(Roles = "SuperAdmin, Admin, Developer")]
        public ActionResult Search(string search, int page = 1, int offset = 10, string userId = "")
        {
            Init();
            userId= User.Identity.GetUserId();
            var UserLists = _userService.UserList(userId);
            ViewBag.UserList = UserLists;


            if (page < 1) page = 1;
            UserListDto results = _userService.UserDetail(search,page, offset);
            //int totalCount = results[0].TotalCount;
            //int totalCount = results.FirstOrDefault()?.TotalCount ?? 0;
            int totalPages = (int)Math.Ceiling((double)results.TotalCount / offset);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.offset = offset;
            ViewBag.TotalCount = results.TotalCount;
            return PartialView("Search", results.UserList);
           
        }

    }
}