namespace change_management.Models
{
    public class SystemEntity
    {
        public int systemID { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string techStack { get; set; }
        public int pointOfContactID {get;set;}
        public int owningTeamID {get;set;}
        public User pointOfContact {get;set;}
        public Team owningTeam {get;set;}

        public SystemEntity() {}

        //Creating a system entity from the database
        public SystemEntity(int id, string systemName, string systemCode, string desc, string tstack, User contact, Team team) {
            systemID = id;
            name = systemName;
            code = systemCode;
            description = desc;
            techStack = tstack;
            pointOfContact = contact;
            owningTeam = team;
        }

        //Initializing a system entity to be added
        public SystemEntity(string systemName, string systemCode, string desc, string tstack, int contact, int team) {
            systemID = 0;
            name = systemName;
            code = systemCode;
            description = desc;
            techStack = tstack;
            pointOfContactID = contact;
            owningTeamID = team;
        }
    }
}