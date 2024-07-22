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
       
        public ActionResult Index(string search)
        {
            var clt = _iInstitutionService.InstitutionList(search);
            return View(clt);
        }

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
        public ActionResult InstitutionRegister()
        {
            Init();
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstitutionRegister(InstNameDto instNameDto, HttpPostedFileBase file)
        {
            instNameDto.CreatedBy = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    // Check file extension if needed
                    var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("", "Only image files are allowed (.jpg, .jpeg, .png, .gif)");
                        ViewBag.TaskStatus = "Error";
                        ViewBag.TaskMessage = "Invalid image file.";
                        return RedirectToAction("InstitutionRegister", "Institution");
                    }

                    instNameDto.ImageUrl = Path.GetFileName(file.FileName);
                    instNameDto.contentType = file.ContentType;

                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        instNameDto.ImageData = binaryReader.ReadBytes(file.ContentLength);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please upload an image file.");
                    ViewBag.TaskStatus = "Error";
                    ViewBag.TaskMessage = "Invalid image file.";
                    return RedirectToAction("Dashboard", "Home");
                }

                try
                {
                    SQLStatusDto sQLStatus = _iInstitutionService.InstitutionCreate(instNameDto);

                    TempData["TaskStatus"] = sQLStatus.Status;
                    TempData["TaskMessage"] = sQLStatus.Message;

                    if (sQLStatus.Status != "00")
                    {
                        ModelState.AddModelError("", "An institution with the same name already exists in the system");
                        ViewBag.TaskStatus = "Error";
                        ViewBag.TaskMessage = sQLStatus.Message;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving institution: " + ex.Message);
                    ViewBag.TaskStatus = "Error";
                    ViewBag.TaskMessage = "Error saving institution.";
                }
            }
            else
            {
                ViewBag.TaskStatus = "Error";
                ViewBag.TaskMessage = "Model validation failed.";
            }

            return RedirectToAction("Dashboard", "Home");
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
            string uploadsDir = Server.MapPath("~/Uploads");

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

        
    }
}