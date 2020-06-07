using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using change_management.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

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
            DatabaseService DatabaseService = new DatabaseService(_configuration);
            List<Team> teams = DatabaseService.databaseSelectTeams("teams");
            ViewData["Message"] = "Team management page.";

            return View(teams);
        }

        public IActionResult AddTeam()
        {
            return View();
        }

        public IActionResult SubmitNewTeam(Team team)
        {
            DatabaseService DatabaseService = new DatabaseService(_configuration);
            DatabaseService.databaseTeamInsert(team.name);
            return RedirectToAction("Teams");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
