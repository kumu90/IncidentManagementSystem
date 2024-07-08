using IncidentManagementSystem.Service;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncidentManagementSystem.Controllers
{
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

        public ActionResult Index(string search)
        {
            var clients = _userService.UserDetail(search);
            return View(clients);
        }
        public ActionResult Search(string search)
        {
            var results = _userService.UserDetail(search);
            return PartialView("Search", results);
        }

    }
}