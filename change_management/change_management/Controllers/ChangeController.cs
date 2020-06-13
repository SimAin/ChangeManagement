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
    public class ChangeController : Controller
    {
        private readonly IConfiguration _configuration;
        
        public ChangeController(IConfiguration configuration){
            _configuration = configuration;
        }

        public IActionResult Changes()
        {
            ChangeDatabaseService dbService = new ChangeDatabaseService(_configuration);
            List<Change> changes = dbService.SelectAll().ToList();
            ViewData["Message"] = "Change management page.";

            return View(changes);
        }

        public IActionResult AddChange()
        {
            var dbusers = new UserDatabaseService(_configuration).SelectAll();
            var dbteams = new TeamDatabaseService(_configuration).SelectAll();
            var dbsystems = new SystemDatabaseService(_configuration).SelectAll();
            List<SelectListItem> teams = new List<SelectListItem>();  
            List<SelectListItem> users = new List<SelectListItem>(); 
            List<SelectListItem> approvers = new List<SelectListItem>();  
            List<SelectListItem> steakholders = new List<SelectListItem>(); 
            List<SelectListItem> systems = new List<SelectListItem>(); 
            
            foreach (var t in dbteams)
            {
                teams.Add(new SelectListItem{
                    Text = t.name,  
                    Value = t.teamID.ToString()
                });
            }

            foreach (var s in dbsystems)
            {
                systems.Add(new SelectListItem{
                    Text = s.name,  
                    Value = s.systemID.ToString()
                });
            } 
            
            foreach (var u in dbusers)
            {
                users.Add(new SelectListItem{
                    Text = u.forename + " " + u.surname,  
                    Value = u.userID.ToString()
                });
                approvers.Add(new SelectListItem{
                    Text = u.forename + " " + u.surname,  
                    Value = u.userID.ToString()
                });
                steakholders.Add(new SelectListItem{
                    Text = u.forename + " " + u.surname,  
                    Value = u.userID.ToString()
                });
            }

            var m = new AddChangeViewModel(systems, approvers, steakholders, teams, users);
            return View(m);
        }

        public IActionResult SubmitNewChange(AddChangeViewModel c)
        {
            var change = new Change(Convert.ToInt32(c.selectedSystem), c.type, c.description, c.criticality, c.deadline, 
                                    Convert.ToInt32(c.priority), Convert.ToInt32(c.selectedApprover), Convert.ToInt32(c.selectedStakeholder), 
                                    Convert.ToInt32(c.selectedTeamResponsible), Convert.ToInt32(c.selectedUserResponsible));
            ChangeDatabaseService dbService = new ChangeDatabaseService(_configuration);
            dbService.Insert(change);
            return RedirectToAction("Changes");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
