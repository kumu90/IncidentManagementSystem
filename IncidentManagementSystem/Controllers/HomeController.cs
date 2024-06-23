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
        public ActionResult Index(string search)
        {

            ////var model = new SearchByDateDto();
            //return View();
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
            //var model = new SearchByDateDto();
            //return View(model);
            //var result =_iInstitutionService.InstDetail();
            //return PartialView("_PartialDashboard", result);
            var clt = _userService.UserDetail(search);
            return View(clt);
        }

        [HttpGet]
        public ActionResult Search (string search)
        {
            //var result = _iInstitutionService.InstDetail();
            //return PartialView("_PartialDashboard", result);
            if (ModelState.IsValid)
            {
                var result = _iInstitutionService.InstDetail(search); 
                return PartialView("Search", result);
            }
            return View();
            //return View(instNameDto);

        }

        public ActionResult Contact()
        {
            return View();
        }

        
    }
}