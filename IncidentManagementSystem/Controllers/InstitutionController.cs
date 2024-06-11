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
        readonly iInstNameService _iInstNameService;
        public InstitutionController() 
        {  
        }
        public InstitutionController(iInstNameService iInstNameServices)
        {
            _iInstNameService = iInstNameServices;
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}