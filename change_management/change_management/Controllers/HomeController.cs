using change_management.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using change_management.Models;
using change_management.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Linq;

namespace change_management.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        
        public HomeController(IConfiguration configuration){
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (SessionService.loggedInUser != null) {
                ViewData["Message"] = "Welcome " + SessionService.loggedInUser.forename + " " + SessionService.loggedInUser.surname;
            }
            UserDatabaseService dbService_u = new UserDatabaseService(_configuration);
            Team myTeam = dbService_u.SelectUserTeam(SessionService.loggedInUser.userID);

            ChangeDatabaseService dbService_c = new ChangeDatabaseService(_configuration);
            List<Change> teamChanges = dbService_c.SelectTeamChanges(myTeam.teamID).ToList();

            ScheduleService scheduleService = new ScheduleService();
            List<Change> orderedTeamChanges = scheduleService.scheduleChanges(teamChanges).ToList();

            var m = new HomeViewModel(myTeam, orderedTeamChanges);
            return View(m);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
