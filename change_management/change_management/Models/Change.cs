using System;

namespace change_management.Models {
    public class Change {
        public int changeId { get; set; }
        public SystemEntity system { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public bool criticality { get; set; }
        public DateTime deadline { get; set; }
        public string deadlineText { get; set; }
        public int deadlineStatus { get; set; }
        public int priority { get; set; }
        public int processingTime { get; set; }
        public string status { get; set; }
        public int laxity { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime? startedDate { get; set; }

        public User approver { get; set; }
        public User stakeholder { get; set; }
        public Team teamResponsible { get; set; }
        public User userResponsible { get; set; }

        public int systemId { get; set; }
        public int approverId { get; set; }
        public int stakeholderId { get; set; }
        public int teamResponsibleId { get; set; }
        public int userResponsibleId { get; set; }
        public int statusId { get; set; }

        public DateTime expectedCompleation {get;set;}

        public Change () { }

        //Creating a change from the database
        public Change (int id, SystemEntity sys, string t, string desc, bool critical, DateTime due, int pri, int pt, DateTime create, DateTime? start, string state, User app, User stake, Team team, User responsible) {
            changeId = id;
            system = sys;
            type = t;
            description = desc;
            criticality = critical;
            deadline = due;
            deadlineText = due.ToString ("dd/MM/yyyy");
            priority = pri;
            processingTime = pt;
            createdDate = create;
            startedDate = start;
            status = state;
            approver = app;
            stakeholder = stake;
            teamResponsible = team;
            userResponsible = responsible;
        }

        //Creating a changelite view from the database
        public Change (int id, SystemEntity sys, string t, string desc, bool critical, DateTime due, int pri, int pt, DateTime create, DateTime? start, string stat, User responsible) {
            changeId = id;
            system = sys;
            type = t;
            description = desc;
            criticality = critical;
            deadline = due;
            deadlineText = due.ToString ("dd/MM/yyyy");
            priority = pri;
            processingTime = pt;
            createdDate = create;
            startedDate = start;
            status = stat;
            userResponsible = responsible;
        }

        //Initializing a change to be added
        public Change (int sys, string t, string desc, bool critical, DateTime due, int pri, int app, int stake, int team, int responsible) {
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

        //Initializing a change to be edited
        public Change (int id, string desc, bool critical, DateTime due, int process, int pri, int app, int stake, int team, int responsible, int status, DateTime? start) {
            changeId = id;
            description = desc;
            criticality = critical;
            deadline = due;
            priority = pri;
            processingTime = process;
            approverId = app;
            stakeholderId = stake;
            teamResponsibleId = team;
            userResponsibleId = responsible;
            statusId = status;
            startedDate = start;
        }
    }
}