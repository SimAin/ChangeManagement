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

        public IActionResult Change(int changeId)
        {
            ChangeDatabaseService dbService = new ChangeDatabaseService(_configuration);
            Change change = dbService.Select(changeId);

            return View(change);
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

        public IActionResult EditChange(int changeId)
        {
            var changedbs = new ChangeDatabaseService(_configuration);
            var change = changedbs.Select(changeId);
            var dbstatus = changedbs.SelectStatuss();
            var dbusers = new UserDatabaseService(_configuration).SelectAll();
            var dbteams = new TeamDatabaseService(_configuration).SelectAll();
            var dbsystems = new SystemDatabaseService(_configuration).SelectAll();
            List<SelectListItem> status = new List<SelectListItem>(); 
            List<SelectListItem> teams = new List<SelectListItem>();  
            List<SelectListItem> users = new List<SelectListItem>(); 
            List<SelectListItem> approvers = new List<SelectListItem>();  
            List<SelectListItem> steakholders = new List<SelectListItem>(); 
            List<SelectListItem> systems = new List<SelectListItem>(); 
            
            foreach (var s in dbstatus)
            {
                if (s.status == change.status) {
                    status.Add(new SelectListItem{
                        Text = s.status,  
                        Value = s.statusID.ToString(),
                        Selected = true
                    });
                } else {
                    status.Add(new SelectListItem{
                        Text = s.status,  
                        Value = s.statusID.ToString()
                    });
                }
            }

            foreach (var t in dbteams)
            {
                if (t.teamID == change.teamResponsible.teamID) {
                    teams.Add(new SelectListItem{
                        Text = t.name,  
                        Value = t.teamID.ToString(),
                        Selected = true
                    });
                } else {
                    teams.Add(new SelectListItem{
                        Text = t.name,  
                        Value = t.teamID.ToString()
                    });
                }
                
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
                if (u.userID == change.userResponsible.userID) {
                    users.Add(new SelectListItem{
                        Text = u.forename + " " + u.surname,  
                        Value = u.userID.ToString(),
                        Selected = true
                    });
                } else {
                    users.Add(new SelectListItem{
                        Text = u.forename + " " + u.surname,  
                        Value = u.userID.ToString()
                    });
                }
                if (u.userID == change.approver.userID) {
                    approvers.Add(new SelectListItem{
                        Text = u.forename + " " + u.surname,  
                        Value = u.userID.ToString(),
                        Selected = true
                    });
                } else {
                    approvers.Add(new SelectListItem{
                        Text = u.forename + " " + u.surname,  
                        Value = u.userID.ToString()
                    });
                }
                if (u.userID == change.stakeholder.userID) {
                    steakholders.Add(new SelectListItem{
                        Text = u.forename + " " + u.surname,  
                        Value = u.userID.ToString(),
                        Selected = true
                    });
                } else {
                    steakholders.Add(new SelectListItem{
                        Text = u.forename + " " + u.surname,  
                        Value = u.userID.ToString()
                    });
                }
            }

            var m = new EditChangeViewModel(change, systems, approvers, steakholders, teams, users, status);
            return View(m);
        }

        public IActionResult SubmitEditChange(EditChangeViewModel c)
        {
            var updatedChange = new Change(c.changeId, c.selectedDescription, 
                                            c.selectedCriticality, c.selectedDeadline, Convert.ToInt32(c.selectedPriority), 
                                            Convert.ToInt32(c.selectedProcessingTime), Convert.ToInt32(c.selectedApprover), Convert.ToInt32(c.selectedStakeholder), 
                                            Convert.ToInt32(c.selectedTeamResponsible), Convert.ToInt32(c.selectedUserResponsible), Convert.ToInt32(c.selectedStatus));

            ChangeDatabaseService dbService = new ChangeDatabaseService(_configuration);
            dbService.Update(updatedChange);
            return RedirectToAction("Changes");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
