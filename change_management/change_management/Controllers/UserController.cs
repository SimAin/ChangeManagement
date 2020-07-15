using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using change_management.Models;
using change_management.Models.ViewModels;
using change_management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace change_management.Controllers {
    public class UserController : Controller {
        private readonly IConfiguration _configuration;

        public UserController (IConfiguration configuration) {
            _configuration = configuration;
        }

        #region Users Admin

        public IActionResult Users () {
            UserDatabaseService dbService = new UserDatabaseService (_configuration);
            List<User> users = dbService.SelectAll ().ToList ();

            return View (users);
        }

        public IActionResult AddUser () {
            return View ();
        }

        public IActionResult SubmitNewUser (User user) {
            UserDatabaseService dbService = new UserDatabaseService (_configuration);
            dbService.Insert (new User (user.forename, user.surname, user.role));
            return RedirectToAction ("Users");
        }

        #endregion

        #region User Login

        public IActionResult Login () {
            return View ();
        }

        public IActionResult userLogin (LoginViewModel u) {
            UserDatabaseService dbService_u = new UserDatabaseService (_configuration);
            UserDatabaseService dbService_t = new UserDatabaseService (_configuration);
            var activeUser = dbService_u.Select (Convert.ToInt32 (u.userId));
            var activeUserTeam = dbService_t.SelectUserTeam (Convert.ToInt32 (u.userId));

            SessionService.loggedInUser = activeUser;
            SessionService.loggedInTeam = activeUserTeam;

            return RedirectToAction ("Index", "Home");
        }

        #endregion

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}