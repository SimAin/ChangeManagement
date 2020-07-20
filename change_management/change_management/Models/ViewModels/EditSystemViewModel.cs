using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace change_management.Models.ViewModels {
    public class EditSystemViewModel {
        public SystemEntity currentSystem { get; set; }
        public int currentSystemId { get; set; }

        public string selectedName { get; set; }
        public string selectedCode { get; set; }
        public string selectedDescription { get; set; }
        public string selectedTechStack { get; set; }
        public IEnumerable<SelectListItem> users { get; set; }
        public IEnumerable<SelectListItem> teams { get; set; }
        public string selectedUser { get; set; }
        public string selectedTeam { get; set; }

        public EditSystemViewModel () { }

        public EditSystemViewModel (SystemEntity system, IEnumerable<SelectListItem> ul, IEnumerable<SelectListItem> tl) {
            users = ul;
            teams = tl;
            currentSystem = system;
            currentSystemId = system.systemID;

            setCurrentValues (system);
        }

        private void setCurrentValues (SystemEntity system) {
            selectedName = system.name;
            selectedCode = system.code;
            selectedDescription = system.description;
            selectedTechStack = system.techStack;
        }
    }
}