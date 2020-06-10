using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using change_management.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace change_management.Controllers
{
    public class TeamController : Controller
    {
        private readonly IConfiguration _configuration;
        
        public TeamController(IConfiguration configuration){
            _configuration = configuration;
        }

        public IActionResult Teams()
        {
            TeamDatabaseService dbService = new TeamDatabaseService(_configuration);
            List<Team> teams = dbService.SelectAll().ToList();
            ViewData["Message"] = "Team management page.";

            return View(teams);
        }

        public IActionResult AddTeam()
        {
            return View();
        }

        public IActionResult SubmitNewTeam(Team team)
        {
            TeamDatabaseService dbService = new TeamDatabaseService(_configuration);
            dbService.Insert(team);
            return RedirectToAction("Teams");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
