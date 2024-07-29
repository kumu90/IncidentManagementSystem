using IncidentManagementSystem.DataAccess;
using IncidentManagementSystem.Model;
using IncidentManagementSystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncidentManagementSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly IInstitutionService _iInstitutionService;
        private readonly IProductService _iproductService;
        public ProductController()
        {

        }
        public ProductController(IProductService iproductService, IInstitutionService iInstitutionService)
        {
            _iproductService = iproductService;
            _iInstitutionService= iInstitutionService;


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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            Init();
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];

            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceDto service)
        {
            Init();
            SQLStatusDto sQLStatus = _iproductService.ServiceCreate(service);

            if (sQLStatus != null)
            {
                TempData["TaskStatus"] = sQLStatus.Status;
                TempData["TaskMessage"] = sQLStatus.Message;

            }
            else
            {
                TempData["TaskStatus"] = "Error";
                TempData["TaskMessage"] = "Service already Exists.";

            }
            ViewBag.TaskStatus = TempData["TaskStatus"];
            ViewBag.TaskMessage = TempData["TaskMessage"];
            return View();

        }
    }
}