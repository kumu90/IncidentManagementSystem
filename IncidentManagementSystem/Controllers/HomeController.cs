using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncidentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
         private readonly IInstitutionService _iInstitutionService;
         private readonly IUserService _userService;
        private readonly IServiceInstutionService _iServiceInstutionService;
        public HomeController(IInstitutionService iInstitutionService, IUserService userService, IServiceInstutionService iServiceInstutionService)
        {
            _iInstitutionService = iInstitutionService;
            _userService = userService;
            _iServiceInstutionService = iServiceInstutionService;
        }

        public HomeController()
        {
        }
        public ActionResult Index(string search)
        {
            var clients = _userService.UserDetail(search);
            return View(clients);
        }

        public ActionResult InfoSearch(string search) 
        {
            var results = _userService.UserDetail(search);
            return PartialView("InfoSearch", results);
        }


        /// <summary>
        /// To Search Institution Information from the DataBase
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Dashboard(string search)
        {
            var clt = _userService.UserDetail(search);
            return View(clt);
        }

        [HttpGet]
        public ActionResult Search (string search)
        {
            if (ModelState.IsValid)
            {
                var result = _iInstitutionService.InstDetail(search); 
                return PartialView("Search", result);
            }
            return View();
        }


        public ActionResult TicketSearch(string search)
        {
            var TicketInfo = _iServiceInstutionService.ticketInfo(search);
            return View(TicketInfo);
        }

        public ActionResult TicketSearchInfo(string search)
        {
            var results = _iServiceInstutionService.ticketInfo(search);
            return PartialView("TicketSearchInfo", results);
        }

        public ActionResult GetTicketDetail(int TicketId)
        {
            var ticketDetail = _iServiceInstutionService.getTicketDetails(TicketId);
            if (ticketDetail == null)
            {
                return HttpNotFound();
            }
            return View(ticketDetail);
            //return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        
    }
}