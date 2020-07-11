using System.Collections.Generic;
using change_management.Models;
using System;

namespace change_management.Models
{
    public class ChangeAudit
    {
        public int auditID { get; set; }
        public string type { get; set; }
        public string comment { get; set; }
        public DateTime auditDate { get; set; }
        public Change changeDetails {get;set;}
        public User updater {get;set;}

        public ChangeAudit() {}

        public ChangeAudit(Change ch, int id, string t, string c, User u, DateTime d) {
            auditID = id;
            type = t;
            comment = c;
            changeDetails = ch;
            updater = u;
            auditDate = d;
        }
    }
}