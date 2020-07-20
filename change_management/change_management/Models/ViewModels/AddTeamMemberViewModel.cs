using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace change_management.Models.ViewModels {
    public class AddTeamMemberViewModel {
        public int teamId { get; set; }
        public IEnumerable<SelectListItem> users { get; set; }
        public string selectedUser { get; set; }
        public string selectedThroughput { get; set; }

        public AddTeamMemberViewModel () { }

        public AddTeamMemberViewModel (IEnumerable<SelectListItem> ul, int teamid) {
            users = ul;
            teamId = teamid;
            selectedThroughput = "5";
        }
    }
}