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
    public class ChangeController : Controller {
        private readonly IConfiguration _configuration;

        public ChangeController (IConfiguration configuration) {
            _configuration = configuration;
        }

        public IActionResult Changes () {
            ChangeDatabaseService dbService = new ChangeDatabaseService (_configuration);
            List<Change> changes = dbService.SelectAll ().ToList ();
            ViewData["Message"] = "Change management page.";

            return View (changes);
        }

        public IActionResult Change (int changeId, int deadlineStatus) {
            ChangeDatabaseService dbService = new ChangeDatabaseService (_configuration);
            Change change = dbService.Select (changeId);
            change.deadlineStatus = deadlineStatus;
            List<ChangeAudit> changeAudit = dbService.SelectChangeAudit (changeId).ToList ();

            var changeView = new ChangeViewModel (change, changeAudit);

            return View (changeView);
        }

        public IActionResult TeamChanges () {
            ChangeDatabaseService dbService_c = new ChangeDatabaseService (_configuration);
            List<Change> teamChanges = dbService_c.SelectTeamPendingChanges (SessionService.loggedInTeam.teamID).ToList ();
            ScheduleService scheduleService = new ScheduleService ();
            List<Change> orderedTeamChanges = scheduleService.scheduleChanges (teamChanges).ToList ();
            scheduleService.calculateDeadlineStatus (orderedTeamChanges);

            var m = new TeamChangesViewModel (orderedTeamChanges, SessionService.loggedInUser.userID);
            return View (m);
        }

        public IActionResult AddChange () {
            var dbusers = new UserDatabaseService (_configuration).SelectAll ();
            var dbteams = new TeamDatabaseService (_configuration).SelectAll ();
            var dbsystems = new SystemDatabaseService (_configuration).SelectAll ();
            List<SelectListItem> teams = new List<SelectListItem> ();
            List<SelectListItem> users = new List<SelectListItem> ();
            List<SelectListItem> approvers = new List<SelectListItem> ();
            List<SelectListItem> steakholders = new List<SelectListItem> ();
            List<SelectListItem> systems = new List<SelectListItem> ();

            foreach (var t in dbteams) {
                teams.Add (new SelectListItem {
                    Text = t.name,
                        Value = t.teamID.ToString ()
                });
            }

            foreach (var s in dbsystems) {
                systems.Add (new SelectListItem {
                    Text = s.name,
                        Value = s.systemID.ToString ()
                });
            }

            users.Add (new SelectListItem {
                Text = "Any user in selected team",
                    Value = "0",
                    Selected = true
            });

            foreach (var u in dbusers) {
                users.Add (new SelectListItem {
                    Text = u.forename + " " + u.surname,
                        Value = u.userID.ToString ()
                });
                approvers.Add (new SelectListItem {
                    Text = u.forename + " " + u.surname,
                        Value = u.userID.ToString ()
                });
                steakholders.Add (new SelectListItem {
                    Text = u.forename + " " + u.surname,
                        Value = u.userID.ToString ()
                });
            }

            var m = new AddChangeViewModel (systems, approvers, steakholders, teams, users);
            return View (m);
        }

        public IActionResult SubmitNewChange (AddChangeViewModel c) {
            var change = new Change (Convert.ToInt32 (c.selectedSystem), c.type, c.description, c.criticality, c.deadline,
                Convert.ToInt32 (c.priority), Convert.ToInt32 (c.selectedApprover), Convert.ToInt32 (c.selectedStakeholder),
                Convert.ToInt32 (c.selectedTeamResponsible), Convert.ToInt32 (c.selectedUserResponsible));
            ChangeDatabaseService dbService = new ChangeDatabaseService (_configuration);
            dbService.Insert (change);
            return RedirectToAction ("Changes");
        }

        public IActionResult EditChange (int changeId) {
            var changedbs = new ChangeDatabaseService (_configuration);
            var change = changedbs.Select (changeId);
            var dbstatus = changedbs.SelectStatuss ();
            var dbusers = new UserDatabaseService (_configuration).SelectAll ();
            var dbteams = new TeamDatabaseService (_configuration).SelectAll ();
            var dbsystems = new SystemDatabaseService (_configuration).SelectAll ();
            List<SelectListItem> status = new List<SelectListItem> ();
            List<SelectListItem> teams = new List<SelectListItem> ();
            List<SelectListItem> users = new List<SelectListItem> ();
            List<SelectListItem> approvers = new List<SelectListItem> ();
            List<SelectListItem> steakholders = new List<SelectListItem> ();
            List<SelectListItem> systems = new List<SelectListItem> ();

            foreach (var s in dbstatus) {
                if (s.status == change.status) {
                    status.Add (new SelectListItem {
                        Text = s.status,
                            Value = s.statusID.ToString (),
                            Selected = true
                    });
                } else {
                    status.Add (new SelectListItem {
                        Text = s.status,
                            Value = s.statusID.ToString ()
                    });
                }
            }

            foreach (var t in dbteams) {
                if (t.teamID == change.teamResponsible.teamID) {
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

            foreach (var s in dbsystems) {
                systems.Add (new SelectListItem {
                    Text = s.name,
                        Value = s.systemID.ToString ()
                });
            }

            users.Add (new SelectListItem {
                Text = "Any user in selected team",
                    Value = "0",
                    Selected = true
            });

            foreach (var u in dbusers) {
                if (u.userID == change.userResponsible.userID) {
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
                if (u.userID == change.approver.userID) {
                    approvers.Add (new SelectListItem {
                        Text = u.forename + " " + u.surname,
                            Value = u.userID.ToString (),
                            Selected = true
                    });
                } else {
                    approvers.Add (new SelectListItem {
                        Text = u.forename + " " + u.surname,
                            Value = u.userID.ToString ()
                    });
                }
                if (u.userID == change.stakeholder.userID) {
                    steakholders.Add (new SelectListItem {
                        Text = u.forename + " " + u.surname,
                            Value = u.userID.ToString (),
                            Selected = true
                    });
                } else {
                    steakholders.Add (new SelectListItem {
                        Text = u.forename + " " + u.surname,
                            Value = u.userID.ToString ()
                    });
                }
            }

            var m = new EditChangeViewModel (change, systems, approvers, steakholders, teams, users, status);
            return View (m);
        }

        public IActionResult SubmitEditChange (EditChangeViewModel c) {
            ChangeDatabaseService dbService = new ChangeDatabaseService (_configuration);
            DateTime? started = null;
            if ((dbService.SelectChangeStatus (c.changeId) != Convert.ToInt32 (c.selectedStatus)) && Convert.ToInt32 (c.selectedStatus) == 2) {
                started = DateTime.Now;
            }

            var updatedChange = new Change (c.changeId, c.selectedDescription,
                c.selectedCriticality, c.selectedDeadline, Convert.ToInt32 (c.selectedPriority),
                Convert.ToInt32 (c.selectedProcessingTime), Convert.ToInt32 (c.selectedApprover), Convert.ToInt32 (c.selectedStakeholder),
                Convert.ToInt32 (c.selectedTeamResponsible), Convert.ToInt32 (c.selectedUserResponsible), Convert.ToInt32 (c.selectedStatus), started);

            dbService.Update (updatedChange, c.comment);
            return RedirectToAction ("Index", "Home");
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}