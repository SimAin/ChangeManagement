using System.Collections.Generic;
using change_management.Models;

namespace change_management.Models
{
    public class Team
    {
        public int teamID { get; set; }
        public string name { get; set; }
        public int throughput {get;set;}
        public IEnumerable<User> teamMembers {get;set;}

        public Team() {}

        public Team(int id, string n, int th) {
            teamID = id;
            name = n;
            throughput = th;
            teamMembers = new List<User>();
        }

        public Team(int id, string n) {
            teamID = id;
            name = n;
            throughput = 0;
            teamMembers = new List<User>();
        }

        public Team(int id, string n, int th, IEnumerable<User> users) {
            teamID = id;
            name = n;
            throughput = th;
            teamMembers = users;
        }
    }
}