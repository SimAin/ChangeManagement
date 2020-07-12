using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace change_management.Models.ViewModels
{
    public class ChangeViewModel
    {
        public IEnumerable<ChangeAudit> changeAudits {get;set;}
        public Change currentChange {get;set;}
        public int confidence {get;set;}

        public ChangeViewModel() {}

        public ChangeViewModel(Change c, IEnumerable<ChangeAudit> a, int con) {
            changeAudits = a;
            currentChange = c;
            confidence = con;
        }
    }
}