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
        public HomeController(IInstitutionService iInstitutionService, IUserService userService)
        {
            _iInstitutionService = iInstitutionService;
            _userService = userService;
        }

        public HomeController()
        {
        }
        public ActionResult Index()
        {

            var model = new SearchByDateDto();
            return View(model);
        }

        public ActionResult InfoSearch(SearchByDateDto search) 
        {
            var results = _userService.UserDetail();
            return PartialView("InfoSearch", results);
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            var model = new SearchByDateDto();
            return View(model);
            //var result =_iInstitutionService.InstDetail();
            //return PartialView("_PartialDashboard", result);
        }

        [HttpGet]
        public ActionResult Search (SearchByDateDto search)
        {
            //var result = _iInstitutionService.InstDetail();
            //return PartialView("_PartialDashboard", result);
            if (ModelState.IsValid)
            {
                var result = _iInstitutionService.InstDetail(); 
                return PartialView("Search", result);
            }
            return View();
            //return View(instNameDto);

        }

    }
}