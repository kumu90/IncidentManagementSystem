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
        public HomeController(IInstitutionService iInstitutionService, IUserService userService )
        {
            _iInstitutionService = iInstitutionService;
            _userService = userService;
            
        }

        public HomeController()
        {
        }
        public ActionResult Index(string search)
        {
            //var clients = _userService.UserDetail(search);
            //return View(clients);
            return View("Dashboard");
        }

        [HttpGet]
        public ActionResult Dashboard(string search)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string search)
        {
            if (ModelState.IsValid)
            {
                var result = _iInstitutionService.InstitutionList(search);
                return PartialView("Search", result);
            }
            return View();
        }


        //public ActionResult TicketSearch(string search)
        //{
        //    var TicketInfo = _iServiceInstutionService.ticketInfo(search);
        //    return View(TicketInfo);
        //}

        //public ActionResult TicketSearchInfo(string search)
        //{
        //    var results = _iServiceInstutionService.ticketInfo(search);
        //    return PartialView("TicketSearchInfo", results);
        //}

        //public ActionResult GetTicketDetail(int TicketId)
        //{
        //    var model = new TicketDto
        //    { TicketId = TicketId };
        //    var ticketDetail = _iServiceInstutionService.getTicketDetails(TicketId);
        //    if (ticketDetail == null)
        //    {
        //        return View();
        //    }
        //    return View(ticketDetail);
        //    //return View();
        //}

        public ActionResult Contact()
        {
            return View();
        }

        
    }
}