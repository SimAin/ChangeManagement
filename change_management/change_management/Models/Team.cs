namespace change_management.Models
{
    public class Team
    {
        public int teamID { get; set; }
        public string name { get; set; }

        public Team(int id, string n) {
            teamID = id;
            name = n;
        }

        public Team() {}
    }
}