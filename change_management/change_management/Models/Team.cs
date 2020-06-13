using System.Collections.Generic;
using change_management.Models;

namespace change_management.Models
{
    public class Team
    {
        public int teamID { get; set; }
        public string name { get; set; }
        public IEnumerable<User> teamMembers {get;set;}

        public Team() {}

        public Team(int id, string n) {
            teamID = id;
            name = n;
        }

        public Team(int id, string n, IEnumerable<User> users) {
            teamID = id;
            name = n;
            teamMembers = users;
        }
    }
}