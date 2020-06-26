using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using change_management.Models;
using change_management.Services;
using change_management.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Linq;

namespace change_management.Controllers
{
    public class SystemController : Controller
    {
        private readonly IConfiguration _configuration;
        
        public SystemController(IConfiguration configuration){
            _configuration = configuration;
        }

        #region System Admin

        public IActionResult Systems()
        {
            SystemDatabaseService dbService = new SystemDatabaseService(_configuration);
            List<SystemEntity> systems = dbService.SelectAll().ToList();
            ViewData["Message"] = "System management page.";

            return View(systems);
        }

        public IActionResult AddSystem()
        {
            var dbusers = new UserDatabaseService(_configuration).SelectAll();
            var dbteams = new TeamDatabaseService(_configuration).SelectAll();
            List<SelectListItem> teams = new List<SelectListItem>();  
            List<SelectListItem> users = new List<SelectListItem>(); 
            
            foreach (var t in dbteams)
            {
                teams.Add(new SelectListItem{
                    Text = t.name,  
                    Value = t.teamID.ToString()
                });
            }
             
            foreach (var u in dbusers)
            {
                users.Add(new SelectListItem{
                    Text = u.forename + " " + u.surname,  
                    Value = u.userID.ToString()
                });
            }

            var m = new AddSystemViewModel(users, teams);
            return View(m);
        }

        public IActionResult SubmitNewSystem(AddSystemViewModel s)
        {
            var system = new SystemEntity(s.name, s.code, s.description, s.techStack, Convert.ToInt32(s.selectedUser), Convert.ToInt32(s.selectedTeam));
            SystemDatabaseService dbService = new SystemDatabaseService(_configuration);
            dbService.Insert(system);
            return RedirectToAction("Systems");
        }

        #endregion

        #region Team Systems

        public IActionResult TeamSystems()
        {
            SystemDatabaseService dbService = new SystemDatabaseService(_configuration);
            List<SystemEntity> systems = dbService.SelectAll(SessionService.loggedInTeam.teamID).ToList();
            ViewData["Message"] = "System management page.";

            return View(systems);
        }

        #endregion

        public IActionResult History(int systemID)
        {
            ChangeDatabaseService dbService = new ChangeDatabaseService(_configuration);
            List<Change> systems = dbService.SelectSystemChanges(systemID).ToList();
            ViewData["Message"] = "System management page.";

            return View(systems);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
