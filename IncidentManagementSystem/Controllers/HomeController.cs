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
        public HomeController(IInstitutionService iInstitutionService)
        {
            _iInstitutionService = iInstitutionService;
        }

        public HomeController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Dashboard()
        {
            var result =_iInstitutionService.InstDetail();
            return PartialView("_PartialDashboard", result);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}