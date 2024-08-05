using IncidentManagementSystem.Service;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace IncidentManagementSystem.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        ///private readonly IInstitutionService _iInstitutionService;
        private readonly IUserService _userService;

        public UserController()
        {

        }
        public UserController(/*IInstitutionService iInstitutionService,*/ IUserService userService)
        {
            ///_iInstitutionService = iInstitutionService;
            _userService = userService;
        }

        [Authorize(Roles = "SuperAdmin, Admin, Developer")]
        public ActionResult Index(string search)
        {
            var clients = _userService.UserDetail(search);
            return View(clients);
        }

        [Authorize(Roles = "SuperAdmin, Admin, Developer")]
        public ActionResult Search(string search, int page = 1, int offset = 10)
        {
            if (page < 1) page = 1;
            var results = _userService.UserDetail(search,page, offset);
            //int totalCount = results[0].TotalCount;
            int totalCount = results.FirstOrDefault()?.TotalCount ?? 0;
            int totalPages = (int)Math.Ceiling((double)totalCount / offset);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.offset = offset;
            ViewBag.TotalCount = totalCount;
            return PartialView("Search", results);
            //var results = _userService.UserDetail(search);
            //return PartialView("Search", results);
        }

    }
}