using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IncidentManagementSystem.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ITicketService _iTicketService;
        private readonly IInstitutionService _iInstitutionService;
        private readonly IProductService _iproductService;

        public TicketController()
        {

        }
        public TicketController(ITicketService iTicketService, IProductService iproductService, IInstitutionService institutionService)
        {
            _iTicketService = iTicketService;
            _iInstitutionService = institutionService;
            _iproductService = iproductService;
        }

        public void Init()
        {
            var institution = _iInstitutionService.GetInstName();
            ViewBag.Institution = new SelectList(institution, "InstId", "InstitutionName");

            List<Roles> role = _iInstitutionService.RoleList();
            ViewBag.UserRole = new SelectList(role, "Id", "Name");

            var services = _iproductService.GetServices();
            ViewBag.services = new SelectList(services, "ServiceId", "serviceName");

            var Issues = _iTicketService.GetIssueList();
            ViewBag.Issues = new SelectList(Issues, "IssueId", "IssueName");
        }

        public JsonResult InstService(string InstId)
        {
            var servId = _iproductService.GetServices(InstId);
            return Json(servId, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ServiceIssue(string ServiceId)
        {
            var Issues = _iTicketService.GetIssueList(ServiceId);
            return Json(Issues, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(string search)
        {
            var TicketInfo = _iTicketService.TicketInfo(search);
            return View(TicketInfo);
        }

        public ActionResult Search(string search)
        {
            var results = _iTicketService.TicketInfo(search);
            return PartialView("Search", results);
        }

        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            Init();
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];
            
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(TicketDto ticketDto , HttpPostedFileBase file)
        {
            Init();
            try
            {
                if (file != null && file.ContentLength > 0)
                {

                    // Check file extension if needed
                    //var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    //if (!allowedExtensions.Contains(fileExtension))
                    //{
                    //    ModelState.AddModelError("", "Only image files are allowed (.jpg, .jpeg, .png, .gif)");
                    //    ViewBag.TaskStatus = "Error";
                    //    ViewBag.TaskMessage = "Invalid image file.";
                    //    return RedirectToAction("Create", "Ticket");
                    //}

                    ticketDto.ImageUrl = Path.GetFileName(file.FileName);
                    ticketDto.contentType = file.ContentType;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        ticketDto.ImageData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    SQLStatusDto sQLStatus = _iTicketService.TicketCreate(ticketDto);


                    if (sQLStatus.Status == "00")
                    {
                        TempData["TaskStatus"] = sQLStatus.Status;
                        TempData["TaskMessage"] = sQLStatus.Message;

                    }
                    else
                    {
                        TempData["TaskStatus"] = sQLStatus.Status;
                        TempData["TaskMessage"] = sQLStatus.Message;
                    }
                    ViewBag.TaskStatus = TempData["TaskStatus"];
                    ViewBag.TaskMessage = TempData["TaskMessage"];
                }
                else
                {
                    ViewBag.Message = "Invalid image file.";
                }
               

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        [Authorize(Roles = "SuperAdmin, Admin, Developer")]
        public ActionResult  TicketDetail(string TicketId)
        {
            var ticketDetail = _iTicketService.GetTicketDetails(TicketId);
            if (!string.IsNullOrEmpty(ticketDetail.ImageUrl))
            {
                //// Assuming ImageData is a byte[] property in your Ticket model
                //if (ticketDetail.ImageUrl != null && ticketDetail.ImageUrl.Length > 0)
                //{
                //    // Return the image data
                //    return View (ticketDetail);
                //}
                if (ticketDetail.ImageData != null && ticketDetail.ImageData.Length > 0)
                {
                    string base64String = Convert.ToBase64String(ticketDetail.ImageData);
                    ticketDetail.ImageUrl = $"data:{ticketDetail.contentType};base64,{base64String}";
                    return View(ticketDetail);
                }

                else
                {
                    // Handle case where image data is null or empty
                    // For example, return a placeholder image or handle appropriately
                    return View("ticketDetail");
                }
            }
            return View();
            //if (ticketDetail == null)
            //{
            //    return View();
            //}
            //return View(ticketDetail);            
        }

        [HttpGet]
        public ActionResult Assign(string TicketId)
        {
            Init();
            var result = _iTicketService.TicketAssign(TicketId);
            if (TempData["TaskStatus"] != null)
            {
                ViewBag.TaskStatus = TempData["TaskStatus"];
                ViewBag.TaskMessage = TempData["TaskMessage"];
            }

            return View(result);
        }

        [HttpPost]
        public ActionResult Assign(TicketAssignDto ticketAssignDto)
        {
            //Init();
            try
            {
                SQLStatusDto sQLStatus = _iTicketService.TicketAssignTo(ticketAssignDto);

                if (sQLStatus.Status == "00")
                {
                    TempData["TaskStatus"] = sQLStatus.Status;
                    TempData["TaskMessage"] = sQLStatus.Message;

                }
                else
                {
                    TempData["TaskStatus"] = sQLStatus.Status;
                    TempData["TaskMessage"] = sQLStatus.Message;
                    ModelState.AddModelError("", "An Ticket with the same name already AssignTo ");

                }
                ViewBag.TaskStatus = TempData["TaskStatus"];
                ViewBag.TaskMessage = TempData["TaskMessage"];

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Assign", "Ticket", new { TicketId = ticketAssignDto.TicketId });
            //return View();
        }

        public ActionResult Delete(string TicketId) 
        {
            return View();
        }
    }
}