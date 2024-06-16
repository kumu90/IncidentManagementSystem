using System;
using System.Collections.Generic;
using System.IO;
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
                // Handle the exception, log it if necessary
                return "99";
            }
        }

    }
}