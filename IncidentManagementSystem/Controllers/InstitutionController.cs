//using System;
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
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;

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
            string userId = User.Identity.GetUserId();
            List<InstNameDto> institution = _iInstitutionService.GetInstName(userId);
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
        public ActionResult Index(/*string search*/)
        {
            //var clt = _iInstitutionService.InstitutionList(search);
            //return View(clt);
            Init();
            return View();
        }

        [Authorize(Roles = ",SuperAdmin, Admin, Developer")]
        public ActionResult Search(string search, int page = 1, int offset = 10, string userId = "")
        {
            //var result = _iInstitutionService.InstitutionList(search);
            //return PartialView("search", result);
            Init();

            userId = User.Identity.GetUserId();
            var Institution = _iInstitutionService.GetInstName(userId);
            ViewBag.Institution = Institution;


            if (page < 1) page = 1;
            InstListDto results = _iInstitutionService.InstitutionList(search, page, offset);
            //int totalCount = results[0].TotalCount;
            //int totalCount = results.FirstOrDefault()?.TotalCount ?? 0;
            int totalPages = (int)Math.Ceiling((double)results.TotalCount / offset);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.offset = offset;
            ViewBag.TotalCount = results.TotalCount;
            return PartialView("Search", results.InstList);

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
            //if (!ModelState.IsValid)
            //{
            //    TempData["TaskStatus"] = "ERROR";
            //    TempData["TaskMessage"] = "FIELD REQUIRED";
            //    return View(instNameDto);
            //}
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
                    TempData["TaskMessage"] = " Institution Details are not Filled Correctly.";

                }
                ViewBag.TaskStatus = TempData["TaskStatus"];
                ViewBag.TaskMessage = TempData["TaskMessage"];
            }
            else
            {
                ViewBag.Message = "Invalid image file.";
            }


            //try
            //{
            //    SQLStatusDto sQLStatus = _iInstitutionService.InstitutionCreate(instNameDto);

            //    TempData["TaskStatus"] = sQLStatus.Status;
            //    TempData["TaskMessage"] = sQLStatus.Message;

            //    if (sQLStatus.Status != "00")
            //    {
            //        ModelState.AddModelError("", "An institution with the same name already exists in the system");
            //        ViewBag.TaskStatus = "Error";
            //        ViewBag.TaskMessage = sQLStatus.Message;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("", "Error saving institution: " + ex.Message);
            //    ViewBag.TaskStatus = "Error";
            //    ViewBag.TaskMessage = "Error saving institution.";
            //}
            //}
            //else
            //{
            //    ViewBag.TaskStatus = "Error";
            //    ViewBag.TaskMessage = "Model validation failed.";
            //}

            return View();
        }

    }
}