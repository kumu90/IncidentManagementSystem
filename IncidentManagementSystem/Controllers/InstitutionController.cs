using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncidentManagementSystem.Controllers
{
    public class InstitutionController : Controller
    {
        // GET: Institution
        public ActionResult InstitutionRegister()
        {
            return View();
        }
    }
}