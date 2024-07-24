using IncidentManagementSystem.Service;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Search(string search)
        {
            var results = _userService.UserDetail(search);
            return PartialView("Search", results);
        }

    }
}