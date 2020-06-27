using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace change_management.Models.ViewModels
{
    public class EditChangeViewModel
    {
        public Change currentChange {get;set;}

        public IEnumerable<SelectListItem> systems {get;set;}
        public IEnumerable<SelectListItem> approver {get;set;}
        public IEnumerable<SelectListItem> stakeholder {get;set;}
        public IEnumerable<SelectListItem> teamResponsible {get;set;}
        public IEnumerable<SelectListItem> userResponsible {get;set;}

        public string selectedDescription { get; set; }
        public bool selectedCriticality { get; set; }
        public DateTime selectedDeadline { get; set; }
        public int selectedPriority {get;set;}
        public int selectedProcessingTime {get;set;}
        public string selectedApprover {get;set;}
        public string selectedStakeholder {get;set;}
        public string selectedTeamResponsible {get;set;}
        public string selectedUserResponsible {get;set;}

        public EditChangeViewModel() {}

        public EditChangeViewModel(Change ch, IEnumerable<SelectListItem> sysl, IEnumerable<SelectListItem> appl, IEnumerable<SelectListItem> steakl, IEnumerable<SelectListItem> tl, IEnumerable<SelectListItem> ul) {
            currentChange = ch;
            systems = sysl;
            approver = appl;
            stakeholder = steakl;            
            userResponsible = ul;
            teamResponsible = tl;
        }
    }
}