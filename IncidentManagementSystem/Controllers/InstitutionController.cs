using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using Microsoft.AspNet.Identity;

namespace IncidentManagementSystem.Controllers
{
    public class InstitutionController : Controller
    {
        readonly IInstitutionService _iInstitutionService;
        readonly IServiceInstutionService _iserviceInstutionService;
        public InstitutionController()
        {
        }

        public InstitutionController(IInstitutionService iInstitutionNameService, IServiceInstutionService serviceInstutionService)
        {
            _iInstitutionService = iInstitutionNameService;
            _iserviceInstutionService = serviceInstutionService;
        }


        public void Init()
        {
            var institution = _iInstitutionService.GetInstName();
            ViewBag.Institution = new SelectList(institution, "InstId", "InstitutionName");

            var services = _iserviceInstutionService.GetServices();
            ViewBag.services = new SelectList(services, "ServiceId", "serviceName");
        }


        public JsonResult InstService(string InstId)
        {
            var servId = _iserviceInstutionService.GetServices(InstId);
            return Json(servId, JsonRequestBehavior.AllowGet);
        }
       

        [HttpGet]
        public ActionResult InstitutionRegister()
        {
            Init();
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstitutionRegister(InstNameDto instNameDto, HttpPostedFileBase image)
        {
            instNameDto.CreatedBy = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                SQLStatusDto sQLStatus = new SQLStatusDto();
                if (image != null && image.ContentLength > 0)
                {
                    string imagePath = UploadImg(image);

                    if (imagePath == "99")
                    {
                        ModelState.AddModelError("", "Invalid image file. Only .jpg, .jpeg, and .png files are allowed.");
                        return View(instNameDto);
                    }

                    instNameDto.ImageUrl = Url.Content(imagePath);
                }

                ////instNameDto.ImageUrl = "";
                sQLStatus = _iInstitutionService.InstNameRegister(instNameDto);


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
            return RedirectToAction("Dashboard", "Home");
            //return View();
        }


        public string UploadImg(HttpPostedFileBase imgFile)
        {
            if (imgFile == null || imgFile.ContentLength <= 0)
            {
                // Returning an error code or message
                return "99";
            }

            string[] validExtensions = { ".jpg", ".jpeg", ".png" };
            string extension = Path.GetExtension(imgFile.FileName).ToLower();

            if (!validExtensions.Contains(extension))
            {
                // Invalid file extension
                return "99";
            }

            string randomFileName = $"{Guid.NewGuid()}{extension}";
            string uploadsDir = Server.MapPath("D:/Projects2024/IncidentManagementSystemProject/Uploads");

            // Ensure the uploads directory exists
            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            string filePath = Path.Combine(uploadsDir, randomFileName);

            try
            {
                imgFile.SaveAs(filePath);
                // Return the relative path to the saved file
                return "~/Uploads/" + randomFileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "99";
        }

        [HttpGet]
        public ActionResult GetService(string InstId)
        {
            //var model = new ServiceDto
            //{ Institution = InstId };
            Init();
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetService(ServiceDto service)
        {
            Init();
            SQLStatusDto sQLStatus = new SQLStatusDto();/* _iserviceInstutionService.AddService(service);*/
            sQLStatus = _iserviceInstutionService.AddService(service);

            if (sQLStatus.Status == "00")
            {
                TempData["TaskStatus"] = sQLStatus.Status;
                TempData["TaskMessage"] = sQLStatus.Message;
                return View();


            }
            else
            {

                TempData["TaskStatus"] = sQLStatus.Status;
                TempData["TaskMessage"] = sQLStatus.Message;
                ModelState.AddModelError("", "An Service with the same name already exists in the system");

            }
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];
            return View();

        }

        ////[HttpGet]
        ////public ActionResult RegisterService()
        ////{
        ////    //var model = new ServiceDto
        ////    //{ Institution = InstId };
        ////    //ViewBag.TaskStatus = TempData["TaskStatus"];
        ////    //ViewBag.TaskMessage = TempData["TaskMessage"];

        ////    return View();
        ////}



        [HttpGet]
        public ActionResult Ticket(string InstId)
        {
            Init();
            return View();
        }


        [HttpPost]
        public ActionResult Ticket(TicketDto _ticketDto)
        {
            try
            {
                SQLStatusDto sQLStatus = _iserviceInstutionService.TicketCreate(_ticketDto);

                if (sQLStatus.Status == "00")
                {
                    TempData["TaskStatus"] = sQLStatus.Status;
                    TempData["TaskMessage"] = sQLStatus.Message;

                }
                else
                {
                    TempData["TaskStatus"] = sQLStatus.Status;
                    TempData["TaskMessage"] = sQLStatus.Message;
                    ModelState.AddModelError("", "An Ticket with the same name already exists in the system");

                }
                ViewBag.TaskStatus = TempData["TaskStatus"];
                ViewBag.TaskMessage = TempData["TaskMessage"];
            }
            catch (Exception ex)
            {
                // ex.AddError
            }
            //Init(_ticketDto);
            return View();



        }
    }
}