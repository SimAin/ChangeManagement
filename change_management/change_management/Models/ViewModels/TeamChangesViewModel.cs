using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace change_management.Models.ViewModels {
    public class TeamChangesViewModel {
        public IEnumerable<Change> changes { get; set; }
        public int userID { get; set; }

        public TeamChangesViewModel () { }

        public TeamChangesViewModel (IEnumerable<Change> c, int ui) {
            changes = c;
            userID = ui;
        }
    }
}