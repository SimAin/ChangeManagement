using System.Collections.Generic;
using change_management.Models;

namespace change_management.Models
{
    public class Status
    {
        public int statusID { get; set; }
        public string status { get; set; }

        public Status() {}

        public Status(int id, string s) {
            statusID = id;
            status = s;
        }
    }
}