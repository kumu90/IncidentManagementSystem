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
         //private readonly IInstitutionService _iInstitutionService;
         //private readonly IUserService _userService;
        public HomeController(/*IInstitutionService iInstitutionService, IUserService userService*/ )
        {
            //_iInstitutionService = iInstitutionService;
            ///_userService = userService;
            
        }

        //public HomeController()
        //{
        //}
        public ActionResult Index()
        {
            return View("Dashboard");
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }

        
        public ActionResult Contact()
        {
            return View();
        }

        
    }
}