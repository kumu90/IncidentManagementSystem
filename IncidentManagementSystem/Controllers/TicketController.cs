using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.ReportingServices.ReportProcessing.OnDemandReportObjectModel;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace IncidentManagementSystem.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        readonly ITicketService _iTicketService;
        readonly IInstitutionService _iInstitutionService;
        readonly IProductService _iproductService;
        readonly IErrorLogService _iErrorLogService;

        public TicketController()
        {

        }
        public TicketController(ITicketService iTicketService, IProductService iproductService, IInstitutionService institutionService, IErrorLogService iErrorLogService)
        {
            _iTicketService = iTicketService;
            _iInstitutionService = institutionService;
            _iproductService = iproductService;
            _iErrorLogService = iErrorLogService;
        }

        public void Init()
        {
            string userId = User.Identity.GetUserId();
            List<InstNameDto> institution = _iInstitutionService.GetInstName(userId);
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
        public ActionResult Index(string status)
        {
            Init();
            ViewBag.Status = status;
            return View();
        }


        [Authorize(Roles = "SuperAdmin, Admin, Developer, User")]
        public ActionResult Search(string search, string InstId, string status, int page = 1, int offset = 10, string userId = "")
         {
            Init();
             userId = User.Identity.GetUserId();
            var Institution = _iInstitutionService.GetInstName(userId);
            //List<InstNameDto> institution = _iInstitutionService.GetInstName(userId);


            bool isSuperAdmin = User.IsInRole("SuperAdmin");

            // Populate institution dropdown based on role
            if (isSuperAdmin)
            {
                ViewBag.Institution = Institution;
                ViewBag.SelectedInstId = "";

            }
            else
            {
                var institutionList = Institution.Select(i => new SelectListItem
                {
                    Value = i.InstId.ToString(),
                    Text = i.InstitutionName
                }).ToList();
                var selectedInstitution = institutionList.FirstOrDefault()?.Value;
                ViewBag.Institution = new SelectList(institutionList, "Value", "Text", selectedInstitution);
                ViewBag.Institution = Institution;
                ViewBag.SelectedInstId = selectedInstitution;


            }

            if (page < 1) page = 1;
            var selectedInstId = string.IsNullOrEmpty(InstId) ? ViewBag.SelectedInstId : InstId;
            SearchDto results = _iTicketService.TicketInfo(search, selectedInstId, status, page, offset, userId);
          
            int totalPages = (int)Math.Ceiling((double)results.TotalCount / offset);

           

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.offset = offset;
            ViewBag.TotalCount = results.TotalCount;
            return PartialView("Search", results.ticketDtos);

        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin, User")]
        //[AllowAnonymous]
        public ActionResult Create(string userId = "")
        {
            Init();
            userId = User.Identity.GetUserId();
            var instDetail = _iTicketService.GetInstDetailSearch(userId);
            bool isSuperAdmin = User.IsInRole("SuperAdmin");

            if (isSuperAdmin)
            {
                List<InstNameDto> institution = _iInstitutionService.GetInstName(userId);
                ViewBag.Institution = new SelectList(institution, "InstId", "InstitutionName");
                ViewBag.SelectedInstId = "";
            }
            else
            {
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
        public ActionResult Create(TicketDto ticketDto, HttpPostedFileBase file)
        {
            Init();
            //if (!ModelState.IsValid)
            //{
            //    TempData["TaskStatus"] = "ERROR";
            //    TempData["TaskMessage"] = "FIELD REQUIRED";
            //    return View(ticketDto);
            //}

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
                        ViewBag.TaskStatus = "Error";
                        ViewBag.Message = "Failed to create ticket. Please try again.";

                    }
                }
                else
                {
                    ViewBag.Message = "Invalid image file.";
                }
                ViewBag.TaskStatus = TempData["TaskStatus"];
                ViewBag.TaskMessage = TempData["TaskMessage"];
                return RedirectToAction("Create", "Ticket");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Extentio,.AddErrorlogs("ControllerName","Action" "ex.Message"):

                var exceptionLog = new ErrorLogDto
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString(),
                    ActionName = this.ControllerContext.RouteData.Values["action"].ToString(),
                    userId = User.Identity.IsAuthenticated ? User.Identity.GetUserId() : null
                };
                _iErrorLogService.LogError(exceptionLog);
            }
            return RedirectToAction("Create", "Ticket");

        }

        [Authorize(Roles = "SuperAdmin, Admin, Developer, User")]
        public ActionResult TicketDetail(string TicketId)
        {
            var ticketDetail = _iTicketService.GetTicketDetails(TicketId);
            if (!string.IsNullOrEmpty(ticketDetail.ImageUrl))
            {
              
                if (ticketDetail.ImageData != null && ticketDetail.ImageData.Length > 0)
                {
                    string base64String = Convert.ToBase64String(ticketDetail.ImageData);
                    ticketDetail.ImageUrl = $"data:{ticketDetail.contentType};base64,{base64String}";
                    return View(ticketDetail);
                }

                else
                {
                    
                    return View("ticketDetail");
                }
            }
            return View();
                   
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
            var result = _iTicketService.GetResolveDetails(TicketId);
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];

            return View(result);
            //return View();
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
            return RedirectToAction("TicketResolve", "Ticket");
        }

        public ActionResult DownloadTicketPdf(string ticketId)
        {
            // Fetch ticket details from the database
            TicketDto ticket = _iTicketService.GetTicketDetails(ticketId);

            if (ticket == null)
            {
                return HttpNotFound();
            }            
            byte[] pdfBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var document = new Document(PageSize.A4))
                {
                    PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Define custom fonts
                    var font = FontFactory.GetFont("Helvetica", 12, Font.NORMAL, BaseColor.BLACK);
                    var headerFont = FontFactory.GetFont("Helvetica", 16, Font.BOLD, BaseColor.BLACK);

                    // Add content to the PDF
                    document.Add(new iTextSharp.text.Paragraph("Ticket Details", headerFont) { Alignment = Element.ALIGN_CENTER });

                    // Create a table with 2 columns
                    var table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 20f,
                        SpacingAfter = 20f
                    };

                    // Define styles for table cells
                    var headerCell = new PdfPCell(new Phrase("Field", headerFont))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        Border = Rectangle.NO_BORDER,
                        Padding = 5f
                    };
                    var valueCell = new PdfPCell(new Phrase("Value", headerFont))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        Border = Rectangle.NO_BORDER,
                        Padding = 5f
                    };

                    table.AddCell(headerCell);
                    table.AddCell(valueCell);

                    // Add rows to the table
                    AddStyledCell(table, "Ticket ID", ticket.TicketId, font);
                    AddStyledCell(table, "Date", ticket.date.ToShortDateString(), font);
                    AddStyledCell(table, "Status", ticket.status, font);
                    AddStyledCell(table, "Institution Name", ticket.InstId, font);
                    AddStyledCell(table, "Service Name", ticket.ServiceId, font);
                    AddStyledCell(table, "Issue", ticket.IssueId, font);
                    AddStyledCell(table, "Cell Number", ticket.CellNumber, font);
                    AddStyledCell(table, "Email", ticket.Email, font);
                    AddStyledCell(table, "Description", ticket.Description, font);
                    AddStyledCell(table,"ImageUrl",ticket.ImageUrl, font);
                    // Add the table to the document
                    document.Add(table);

                    // Add an image if present
                    if (ticket.ImageData != null && ticket.ImageData.Length > 0)
                    {
                        var image = iTextSharp.text.Image.GetInstance(ticket.ImageData);
                        image.ScaleToFit(140f, 120f); // Adjust size as needed
                        image.Alignment = Element.ALIGN_CENTER;
                        document.Add(image);
                    }

                    document.Close();
                }

                pdfBytes = memoryStream.ToArray();
            }

            // Return the PDF as a file download
            return File(pdfBytes, "application/pdf", $"Ticket_{ticketId}.pdf");
        }

        private void AddStyledCell(PdfPTable table, string headerText, string valueText, Font font)
        {
            // Add the header cell
            var headerCell = new PdfPCell(new Phrase(headerText, font))
            {
                Border = Rectangle.NO_BORDER,
                Padding = 5f
            };
            table.AddCell(headerCell);

            // Add the value cell
            var valueCell = new PdfPCell(new Phrase(valueText, font))
            {
                Border = Rectangle.NO_BORDER,
                Padding = 5f
            };
            table.AddCell(valueCell);
        }
    
                
    }
}