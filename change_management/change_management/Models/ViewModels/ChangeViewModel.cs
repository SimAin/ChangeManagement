using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace change_management.Models.ViewModels {
    public class ChangeViewModel {
        public IEnumerable<ChangeAudit> changeAudits { get; set; }
        public Change currentChange { get; set; }

        public ChangeViewModel () { }

        public ChangeViewModel (Change c, IEnumerable<ChangeAudit> a) {
            changeAudits = a;
            currentChange = c;
        }
    }
}