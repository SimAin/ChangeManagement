using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using change_management.Models;
using change_management.Models.ViewModels;
using change_management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace change_management.Controllers {
    public class HomeController : Controller {
        private readonly IConfiguration _configuration;

        public HomeController (IConfiguration configuration) {
            _configuration = configuration;
        }

        public IActionResult Index () {
            if (SessionService.loggedInUser != null) {
                ViewData["Message"] = "Welcome " + SessionService.loggedInUser.forename + " " + SessionService.loggedInUser.surname;
            }

            ChangeDatabaseService dbService_c = new ChangeDatabaseService (_configuration);
            List<Change> teamChanges = dbService_c.SelectTeamPendingChanges (SessionService.loggedInTeam.teamID).ToList ();

            TeamDatabaseService dbService_t = new TeamDatabaseService (_configuration);
            SessionService.loggedInTeam.teamMembers = dbService_t.SelectAllMembers (SessionService.loggedInTeam.teamID).ToList ();

            ScheduleService scheduleService = new ScheduleService ();
            List<Change> orderedTeamChanges = scheduleService.scheduleChanges (teamChanges).ToList ();
            scheduleService.calculateDeadlineStatus (orderedTeamChanges);

            var m = new HomeViewModel (orderedTeamChanges, SessionService.loggedInUser.userID);
            return View (m);
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}