using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace change_management.Models.ViewModels {
    public class AddSystemViewModel {
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string techStack { get; set; }
        public IEnumerable<SelectListItem> users { get; set; }
        public IEnumerable<SelectListItem> teams { get; set; }
        public string selectedUser { get; set; }
        public string selectedTeam { get; set; }

        public AddSystemViewModel () { }

        public AddSystemViewModel (IEnumerable<SelectListItem> ul, IEnumerable<SelectListItem> tl) {
            users = ul;
            teams = tl;
        }
    }
}