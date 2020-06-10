namespace change_management.Models
{
    public class User
    {
        public int userID { get; set; }
        public string forename { get; set; }
        public string surname { get; set; }
        public string role { get; set; }

        public User() {}

        public User(int id, string fname, string sname, string r) {
            userID = id;
            forename = fname;
            surname = sname;
            role = r;
        }

        public User(string fname, string sname, string r) {
            userID = 0;
            forename = fname;
            surname = sname;
            role = r;
        }
    }
}