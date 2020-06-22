using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using change_management.Models;
using change_management.Models.ViewModels;
using change_management.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

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
            List<Team> teams = dbService.SelectAllMembers().ToList();

            ViewData["Message"] = "Team management page.";

            return View(teams);
        }

        public IActionResult Team()
        {
            UserDatabaseService dbService_u = new UserDatabaseService(_configuration);
            Team team = dbService_u.SelectUserTeam(SessionService.loggedInUser.userID);

            ViewData["Message"] = "Team management page.";

            return View(team);
        }

        public IActionResult TeamMembers(int id)
        {
            TeamDatabaseService dbService = new TeamDatabaseService(_configuration);
            List<User> users = dbService.SelectAllMembers(id).ToList();
            ViewBag.teamId = id;
            ViewBag.teamName = dbService.SelectTeam(id);
            return View(users);
        }

        public IActionResult AddTeamMember(int teamId)
        {
            UserDatabaseService dbService = new UserDatabaseService(_configuration);
            List<User> dbusers = dbService.SelectAll().ToList();
            List<SelectListItem> usersSelect = new List<SelectListItem>(); 
             
            foreach (var u in dbusers)
            {
                usersSelect.Add(new SelectListItem{
                    Text = u.forename + " " + u.surname,  
                    Value = u.userID.ToString()
                });
            }
            var m = new AddTeamMemberViewModel(usersSelect, teamId);
            return View(m);
        }

        public IActionResult SubmitNewUser(AddTeamMemberViewModel teamMember)
        {
            TeamDatabaseService dbService = new TeamDatabaseService(_configuration);
            dbService.InsertUser(teamMember.teamId, Convert.ToInt32(teamMember.selectedUser));
            return RedirectToAction("Teams");
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
