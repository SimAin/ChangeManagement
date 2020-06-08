namespace change_management.Models
{
    public class SystemEntity
    {
        public int systemID { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string techStack { get; set; }
        public int pointOfContact { get; set; }
        public int owningTeam { get; set; }

        public SystemEntity(int id, string systemName, string systemCode, string desc, string tstack, int contact, int team) {
            systemID = id;
            name = systemName;
            code = systemCode;
            description = desc;
            techStack = tstack;
            pointOfContact = contact;
            owningTeam = team;
        }

        public SystemEntity() {}
    }
}