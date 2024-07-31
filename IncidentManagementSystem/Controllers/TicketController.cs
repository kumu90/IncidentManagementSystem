using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

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


            // Check if the current user is a SuperAdmin
            bool isSuperAdmin = User.IsInRole("SuperAdmin");

            ViewBag.IsSuperAdmin = isSuperAdmin;
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

        [Authorize(Roles = "SuperAdmin, Admin, Developer, User")]
        public ActionResult Index(string search)
        {
            Init();
            var TicketInfo = _iTicketService.TicketInfo(search);
            return View(TicketInfo);
        }


        [Authorize(Roles = "SuperAdmin, Admin, Developer")]
        public ActionResult Search(string search,string InstId,string status)
        {
            var results = _iTicketService.TicketInfo(search,InstId,status);
            return PartialView("Search", results);
        }

        
        [HttpGet]
        [Authorize(Roles = "SuperAdmin, User")]
        //[AllowAnonymous]
        public ActionResult Create(string UserName)
        {
            Init();
            // Debugging or logging
            System.Diagnostics.Debug.WriteLine($"Received UserName parameter: {UserName}");
            

            // Check if UserName is provided; if not, try to retrieve from session
            if (string.IsNullOrEmpty(UserName))
            {
                UserName = Session["Username"] as string;
            }

            // If UserName is still null or empty, redirect to login or show an error
            if (string.IsNullOrEmpty(UserName))
            {
                TempData["TaskStatus"] = "Error";
                TempData["TaskMessage"] = "UserName is required. Please log in.";
                return RedirectToAction("Login", "Account");
            }

            var instDetail  =_iTicketService.GetInstDetail(UserName);
             
            if (instDetail == null)
            {
                TempData["TaskStatus"] = "Error";
                TempData["TaskMessage"] = "No institution details found for the given UserName.";
                return RedirectToAction("Index"); // Redirect to an appropriate action or view
            }

            // Determine if the user is a SuperAdmin
            bool isSuperAdmin = User.IsInRole("SuperAdmin");


            // Populate institution dropdown based on role
            if (isSuperAdmin)
            {
                var institutions = _iInstitutionService.GetInstName();
                ViewBag.Institution = new SelectList(institutions, "InstId", "InstitutionName");
                ViewBag.SelectedInstId = "";
            }
            else
            {
                // For non-SuperAdmin users, only their institution is displayed
                var institution = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = instDetail.InstId.ToString(),
                        Text = instDetail.InstId
                    }
                };
                        ViewBag.Institution = new SelectList(institution, "Value", "Text");
                     ViewBag.SelectedInstId = instDetail.InstId.ToString();
            }

            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];


            return View(instDetail);
        }

        
        [HttpPost]
        //[AllowAnonymous]
        public ActionResult Create(TicketDto ticketDto , HttpPostedFileBase file)
        {
            Init();
            try
            {
                if (file != null && file.ContentLength > 0)
                {

                    ticketDto.ImageUrl = Path.GetFileName(file.FileName);
                    ticketDto.contentType = file.ContentType;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        ticketDto.ImageData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    SQLStatusDto sQLStatus = _iTicketService.TicketCreate(ticketDto);


                    if (sQLStatus != null)
                    {
                        TempData["TaskStatus"] = sQLStatus.Status;
                        TempData["TaskMessage"] = sQLStatus.Message;
                 
                    }
                    else
                    {
                        TempData["TaskStatus"] = "Error";
                        TempData["TaskMessage"] = "Your Details are not Filled Correctly.";

                    }
                }
                else
                {
                    ViewBag.Message = "Invalid image file.";
                }
                ViewBag.TaskStatus = TempData["TaskStatus"];
                ViewBag.TaskMessage = TempData["TaskMessage"];
                return View();              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        [Authorize(Roles = "SuperAdmin, Admin, Developer, User")]
        public ActionResult TicketDetail(string TicketId)
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

                if (sQLStatus != null)
                {
                    TempData["TaskStatus"] = sQLStatus.Status;
                    TempData["TaskMessage"] = sQLStatus.Message;

                }
                else
                {
                    TempData["TaskStatus"] = "Error";
                    TempData["TaskMessage"] = "Ticket Not Register";

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Assign", "Ticket", new { TicketId = ticketAssignDto.TicketId });
        }

        public ActionResult TicketReject(string TicketId)
        {
            var result = _iTicketService.TicketReject(TicketId);
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];

            return RedirectToAction("Index", result);
        }

        public ActionResult TicketResolve(string TicketId)
        {
            Init();
            //var result = _iTicketService.GetResolveDetails(TicketId);
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];

            //return View(result);
            return View();
        }

        [HttpPost]
        public ActionResult TicketResolve(ResolvedByDto resolvedByDto)
        {
            Init();
            try
            {
                SQLStatusDto sQLStatus = _iTicketService.TicketResolveBy(resolvedByDto);

                if (sQLStatus != null)
                {
                    TempData["TaskStatus"] = sQLStatus.Status;
                    TempData["TaskMessage"] = sQLStatus.Message;

                }
                else
                {
                    TempData["TaskStatus"] = "Missing Ticket";
                    TempData["TaskMessage"] = "Ticket Not Found.";

                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

    }
}