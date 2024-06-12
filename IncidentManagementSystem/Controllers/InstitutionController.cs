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
        readonly IGetInstNameService _iGetInstNameService;
        public InstitutionController()
        {
        }
        public InstitutionController(IInstNameService iInstNameServices, IGetInstNameService iGetInstNameService)
        {
            _iInstNameService = iInstNameServices;
            _iGetInstNameService = iGetInstNameService;
        }
        
        public void initialize()
        {
            var InsId = _iGetInstNameService.GetInstName();
            ViewBag.InsName = new SelectList(InsId, "InstId", "InstitutionName");
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