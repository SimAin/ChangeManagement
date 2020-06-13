using System;

namespace change_management.Models
{
    public class Change
    {
        public int changeId { get; set; }
        public SystemEntity system {get;set;}
        public string type { get; set; }
        public string description { get; set; }
        public int criticality { get; set; }
        public DateTime deadline { get; set; }
        public int priority {get;set;}
        public User approver {get;set;}
        public User stakeholder {get;set;}
        public Team teamResponsible {get;set;}
        public User userResponsible {get;set;}
        public int systemId {get;set;}
        public int approverId {get;set;}
        public int stakeholderId {get;set;}
        public int teamResponsibleId {get;set;}
        public int userResponsibleId {get;set;}

        public Change() {}

        //Creating a system entity from the database
        public Change(int id, SystemEntity sys, string t, string desc, int critical, DateTime due, int pri, User app, User stake, Team team, User responsible) {
            changeId = id;
            system = sys;
            type = t;
            description = desc;
            criticality = critical;
            deadline = due;
            priority = pri;
            approver = app;
            stakeholder = stake;
            teamResponsible = team;
            userResponsible = responsible;
        }

        //Initializing a system entity to be added
        public Change(int sys, string t, string desc, int critical, DateTime due, int pri, int app, int stake, int team, int responsible) {
            changeId = 0;
            systemId = sys;
            type = t;
            description = desc;
            criticality = critical;
            deadline = due;
            priority = pri;
            approverId = app;
            stakeholderId = stake;
            teamResponsibleId = team;
            userResponsibleId = responsible;
        }
    }
}