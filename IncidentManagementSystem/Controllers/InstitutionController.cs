using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using IncidentManagementSystem.DataAccess;
using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using Microsoft.AspNet.Identity;

namespace IncidentManagementSystem.Controllers
{
    [Authorize]
    public class InstitutionController : Controller
    {
        readonly IInstitutionService _iInstitutionService;
        readonly IProductService _iproductService;
        public InstitutionController()
        {
        }

        public InstitutionController(IInstitutionService iInstitutionNameService, IProductService iproductService)
        {
            _iInstitutionService = iInstitutionNameService;
            _iproductService = iproductService;
        }


        public void Init()
        {
            var institution = _iInstitutionService.GetInstName();
            ViewBag.Institution = new SelectList(institution, "InstId", "InstitutionName");

            var services = _iproductService.GetServices();
            ViewBag.services = new SelectList(services, "ServiceId", "serviceName");
        }


        public JsonResult InstService(string InstId)
        {
            var servId = _iproductService.GetServices(InstId);
            return Json(servId, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SuperAdmin, Admin, Developer")]
        public ActionResult Index(string search)
        {
            var clt = _iInstitutionService.InstitutionList(search);
            return View(clt);
        }

        [Authorize(Roles = ",SuperAdmin, Admin, Developer")]
        public ActionResult Search(string search)
        {
            if (ModelState.IsValid)
            {
                var result = _iInstitutionService.InstitutionList(search);
                return PartialView("search", result);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult InstitutionRegister()
        {
            Init();
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult InstitutionRegister(InstNameDto instNameDto, HttpPostedFileBase file)
        {
            Init();
            instNameDto.CreatedBy = User.Identity.GetUserId();

            if (file != null && file.ContentLength > 0)
            {

                instNameDto.ImageUrl = Path.GetFileName(file.FileName);
                instNameDto.contentType = file.ContentType;
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    instNameDto.ImageData = binaryReader.ReadBytes(file.ContentLength);
                }

                SQLStatusDto sQLStatus = _iInstitutionService.InstitutionCreate(instNameDto);


                if (sQLStatus != null)
                {
                    TempData["TaskStatus"] = sQLStatus.Status;
                    TempData["TaskMessage"] = sQLStatus.Message;

                }
                else
                {
                    TempData["TaskStatus"] = "Error";
                    TempData["TaskMessage"] = "Institution Register Fail";
                }
            }
            else
            {
                ViewBag.Message = "Invalid image file.";
            }

            return View();
        }

    }
}