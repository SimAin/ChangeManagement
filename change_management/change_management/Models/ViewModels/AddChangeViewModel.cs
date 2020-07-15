using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace change_management.Models.ViewModels {
    public class AddChangeViewModel {
        public int changeId { get; set; }
        public IEnumerable<SelectListItem> systems { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public bool criticality { get; set; }
        public DateTime deadline { get; set; }
        public int priority { get; set; }
        public int processingTime { get; set; }
        public IEnumerable<SelectListItem> approver { get; set; }
        public IEnumerable<SelectListItem> stakeholder { get; set; }
        public IEnumerable<SelectListItem> teamResponsible { get; set; }
        public IEnumerable<SelectListItem> userResponsible { get; set; }
        public string selectedApprover { get; set; }
        public string selectedStakeholder { get; set; }
        public string selectedTeamResponsible { get; set; }
        public string selectedUserResponsible { get; set; }
        public string selectedSystem { get; set; }

        public AddChangeViewModel () { }

        public AddChangeViewModel (IEnumerable<SelectListItem> sysl, IEnumerable<SelectListItem> appl, IEnumerable<SelectListItem> steakl, IEnumerable<SelectListItem> tl, IEnumerable<SelectListItem> ul) {
            systems = sysl;
            approver = appl;
            stakeholder = steakl;
            userResponsible = ul;
            teamResponsible = tl;
        }
    }
}