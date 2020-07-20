using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace change_management.Models.ViewModels {
    public class HomeViewModel {
        public IEnumerable<Change> changes { get; set; }
        public int userID { get; set; }

        public HomeViewModel () { }

        public HomeViewModel (IEnumerable<Change> c, int ui) {
            changes = c;
            userID = ui;
        }
    }
}