using change_management.Models;

namespace change_management.Services
{
    public class SessionService
    {
        private static SessionService sessionService; 
        public static User loggedInUser {get;set;}
        public static Team loggedInTeam {get;set;}
         
        private SessionService(){}

        public static SessionService Instance () { 
        if (sessionService == null) {                
            sessionService = new SessionService();  
        }
            return sessionService;
        }

        public static void setLoggedInUser (User newUser) { 
            loggedInUser = newUser;
        }

        public static User getLoggedInUser () { 
            return loggedInUser;
        }

        public static void setLoggedInTeam (Team newTeam) { 
            loggedInTeam = newTeam;
        }

        public static Team getLoggedInTeam () { 
            return loggedInTeam;
        }
    }
}