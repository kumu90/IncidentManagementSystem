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
        readonly IInstNameService _iInstNameService;
        public InstitutionController()
        {
        }
        public InstitutionController(IInstNameService iInstNameServices)
        {
            _iInstNameService = iInstNameServices;
        }

        [HttpGet]
        public ActionResult InstitutionRegister()
        {
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];
            return View();
        }
    }
}