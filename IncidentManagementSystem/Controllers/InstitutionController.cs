using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using Microsoft.AspNet.Identity;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstitutionRegister(InstNameDto instNameDto,HttpPostedFileBase image)
        {
            instNameDto.CreatedBy = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                SQLStatusDto sQLStatus = new SQLStatusDto();

                instNameDto.ImageUrl = "https://cmstest.qpaysolutions.net/File/LoadImage?type=Institution&id=84ca03bd-e089-441f-b695-da4651cd0b54.png";
                sQLStatus = _iInstNameService.InstNameRegister(instNameDto);


                if (sQLStatus.Status == "00")
                {
                    TempData["TaskStatus"] = sQLStatus.Status;
                    TempData["TaskMessage"] = sQLStatus.Message;


                }
                else
                {

                    TempData["TaskStatus"] = sQLStatus.Status;
                    TempData["TaskMessage"] = sQLStatus.Message;
                    ModelState.AddModelError("", "An institution with the same name already exists in the system");

                }
            }
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];
            return View();
        }
    }
}