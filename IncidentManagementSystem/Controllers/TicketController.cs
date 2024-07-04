﻿using IncidentManagementSystem.Model;
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
            var TicketInfo = _iTicketService.ticketInfo(search);
            return View(TicketInfo);
        }

        public ActionResult Search(string search)
        {
            var results = _iTicketService.ticketInfo(search);
            return PartialView("Search", results);
        }


        [HttpGet]
        public ActionResult Create(ServiceDto serviceDto)
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

            }
            catch (Exception ex)
            {
                // ex.AddError
            }
            //return RedirectToAction("TicketDetail");
            return View();
        }

        [HttpGet]
        public ActionResult TicketDetail(int TicketId)
        {
            var model = new TicketDto
            { TicketId = TicketId };
            var ticketDetail = _iTicketService.getTicketDetails(TicketId);
            if (ticketDetail == null)
            {
                return View();
            }
            return View(ticketDetail);
            //return View();
        }

        [HttpGet]
        public ActionResult Detail()
        {
            //var model = new TicketDto { TicketId = TicketId };
            return View();
        }
    }
}