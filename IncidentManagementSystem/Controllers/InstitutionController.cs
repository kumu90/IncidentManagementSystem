using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;

namespace IncidentManagementSystem.Controllers
{
    public class InstitutionController : Controller
    {
        // GET: Institution
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistInstName()
        {
            return View();
        }

        public ActionResult GetInstName() 
        {
            return View();
        }
    }
}