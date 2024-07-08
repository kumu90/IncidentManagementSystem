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
            //var model = new ServiceDto
            //{ InstId = InstId };
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
    }
}