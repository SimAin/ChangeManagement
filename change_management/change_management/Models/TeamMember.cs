using System.Collections.Generic;
using change_management.Models;

namespace change_management.Models {
    public class TeamMember {
        public User user { get; set; }
        public int throughput { get; set; }
        public int userBookedDays {get; set;}

        public TeamMember () { }

        public TeamMember (User u, int tp) {
            user = u;
            throughput = tp;
        }
    }
}