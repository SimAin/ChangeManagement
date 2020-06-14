using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace change_management.Models.ViewModels
{
    public class HomeViewModel
    {
        public Team team {get;set;}
        public IEnumerable<Change> changes {get;set;}

        public HomeViewModel() {}

        public HomeViewModel(Team t, IEnumerable<Change> c) {
            team = t;
            changes = c;
        }
    }
}