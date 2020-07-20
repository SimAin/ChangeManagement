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
    public class SystemController : Controller {
        private readonly IConfiguration _configuration;

        public SystemController (IConfiguration configuration) {
            _configuration = configuration;
        }

        #region System Admin

        public IActionResult Systems () {
            SystemDatabaseService dbService = new SystemDatabaseService (_configuration);
            List<SystemEntity> systems = dbService.SelectAll ().ToList ();
            ViewData["Message"] = "System management page.";

            return View (systems);
        }

        public IActionResult AddSystem () {
            var dbusers = new UserDatabaseService (_configuration).SelectAll ();
            var dbteams = new TeamDatabaseService (_configuration).SelectAll ();
            List<SelectListItem> teams = new List<SelectListItem> ();
            List<SelectListItem> users = new List<SelectListItem> ();

            foreach (var t in dbteams) {
                teams.Add (new SelectListItem {
                    Text = t.name,
                        Value = t.teamID.ToString ()
                });
            }

            foreach (var u in dbusers) {
                users.Add (new SelectListItem {
                    Text = u.forename + " " + u.surname,
                        Value = u.userID.ToString ()
                });
            }

            var m = new AddSystemViewModel (users, teams);
            return View (m);
        }

        public IActionResult SubmitNewSystem (AddSystemViewModel s) {
            var system = new SystemEntity (s.name, s.code, s.description, s.techStack, Convert.ToInt32 (s.selectedUser), Convert.ToInt32 (s.selectedTeam));
            SystemDatabaseService dbService = new SystemDatabaseService (_configuration);
            dbService.Insert (system);
            return RedirectToAction ("Systems");
        }

        public IActionResult EditSystem (int systemId) {
            var system = new SystemDatabaseService (_configuration).Select (systemId);
            var dbusers = new UserDatabaseService (_configuration).SelectAll ();
            var dbteams = new TeamDatabaseService (_configuration).SelectAll ();
            List<SelectListItem> teams = new List<SelectListItem> ();
            List<SelectListItem> users = new List<SelectListItem> ();

            foreach (var t in dbteams) {
                if (t.teamID == system.owningTeam.teamID) {
                    teams.Add (new SelectListItem {
                        Text = t.name,
                            Value = t.teamID.ToString (),
                            Selected = true
                    });
                } else {
                    teams.Add (new SelectListItem {
                        Text = t.name,
                            Value = t.teamID.ToString ()
                    });
                }
            }

            foreach (var u in dbusers) {
                if (u.userID == system.pointOfContact.userID) {
                    users.Add (new SelectListItem {
                        Text = u.forename + " " + u.surname,
                            Value = u.userID.ToString (),
                            Selected = true
                    });
                } else {
                    users.Add (new SelectListItem {
                        Text = u.forename + " " + u.surname,
                            Value = u.userID.ToString ()
                    });
                }
            }

            var m = new EditSystemViewModel (system, users, teams);
            return View (m);
        }

        public IActionResult SubmitEditSystem (EditSystemViewModel s) {
            var system = new SystemEntity (s.currentSystemId, s.selectedName, s.selectedCode, s.selectedDescription, s.selectedTechStack, Convert.ToInt32 (s.selectedUser), Convert.ToInt32 (s.selectedTeam));
            SystemDatabaseService dbService = new SystemDatabaseService (_configuration);
            dbService.Update (system);
            return RedirectToAction ("TeamSystems");
        }

        #endregion

        #region Team Systems

        public IActionResult TeamSystems () {
            SystemDatabaseService dbService = new SystemDatabaseService (_configuration);
            List<SystemEntity> systems = dbService.SelectAll (SessionService.loggedInTeam.teamID).ToList ();
            ViewData["Message"] = "System management page.";

            return View (systems);
        }

        #endregion

        public IActionResult History (int systemID) {
            ChangeDatabaseService dbService = new ChangeDatabaseService (_configuration);
            List<Change> systems = dbService.SelectSystemChanges (systemID).ToList ();
            ViewData["Message"] = "System management page.";

            return View (systems);
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}