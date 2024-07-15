using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace IncidentManagementSystem.Controllers
{
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
            ViewBag.UserRole = new SelectList(role, "Name", "Name");

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
        public ActionResult Create()
        {
            //var model = new ServiceDto
            //{ InstId = InstId };
            Init();
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];

            return View();
        }


        [HttpPost]
        public ActionResult Create(TicketDto ticketDto)
        {
            Init();
            try
            {
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
                    ModelState.AddModelError("", "An Ticket with the same name already exists in the system");

                }
                ViewBag.TaskStatus = TempData["TaskStatus"];
                ViewBag.TaskMessage = TempData["TaskMessage"];
                return View("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        public ActionResult TicketDetail(string TicketId)
        {
            var ticketDetail = _iTicketService.GetTicketDetails(TicketId);
            if (ticketDetail == null)
            {
                return View();
            }
            return View(ticketDetail);            
        }

        [HttpGet]
        public ActionResult Assign(string TicketId)
        {
            Init();
            var result = _iTicketService.TicketAssign(TicketId);
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];

            return View(result);
        }

        [HttpPost]
        public ActionResult Assign(TicketAssignDto ticketAssignDto)
        {
            Init();
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
                return View("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

    }
}