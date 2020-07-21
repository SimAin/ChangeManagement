using change_management.Models;

namespace change_management.Services {
    public class SessionService {
        private static SessionService sessionService;
        public static User loggedInUser { get; set; }
        public static Team loggedInTeam { get; set; }
        public static bool admin {get;set;}

        private SessionService () { }

        public static SessionService Instance () {
            if (sessionService == null) {
                sessionService = new SessionService ();
            }
            return sessionService;
        }
    }
}