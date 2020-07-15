using NUnit.Framework;
using change_management.Services;
using change_management.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Tests
{
    public class scheduleServiceTeamTimeTest
    {
        private ScheduleService _scheduleService;
        private User _userA = new User(1, "1_Test", "1_User", "a_role");
        private User _userB = new User(2, "2_Test", "2_User", "a_role");
        private User _userC = new User(3, "3_Test", "3_User", "a_role");     
        private Team _team = new Team(1, "newTeam", 15 );

        [SetUp]
        public void SetUp()
        {
            _scheduleService = new ScheduleService();
            _team.teamMembers = new List<TeamMember>(){
                new TeamMember(_userA, 5),
                new TeamMember(_userB, 5),
                new TeamMember(_userC, 5)
            };
        }

        private List<Change> generateTestChanges() {


            var changeList = new List<Change>();

            changeList.Add(new Change(1, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(24), 3, 5, DateTime.Now.AddDays(-5), DateTime.Now, "In progress", new User(), new User(), new Team(), _userA));
            changeList.Add(new Change(4, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(14), 1, 4, DateTime.Now.AddDays(-2), null, "Not Started", new User(), new User(), new Team(), _userA));
            changeList.Add(new Change(7, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-4), null, "Not Started", new User(), new User(), new Team(),_userA));
            changeList.Add(new Change(10, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(12), 2, 6, DateTime.Now.AddDays(-5), null,"Not Started", new User(), new User(), new Team(), _userA));

            changeList.Add(new Change(2, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-4), null, "Not Started", new User(), new User(), new Team(), _userB));
            changeList.Add(new Change(5, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(12), 2, 6, DateTime.Now.AddDays(-5), DateTime.Now, "In progress", new User(), new User(), new Team(), _userB));
            changeList.Add(new Change(8, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(25), 2, 6, DateTime.Now.AddDays(-3), null, "Not Started", new User(), new User(), new Team(), _userB));

            changeList.Add(new Change(3, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(25), 2, 6, DateTime.Now.AddDays(-3), DateTime.Now, "In progress", new User(), new User(), new Team(), _userC));
            changeList.Add(new Change(6, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-5), null, "Not Started", new User(), new User(), new Team(), _userC));
            changeList.Add(new Change(9, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(14), 1, 4, DateTime.Now.AddDays(-2), null, "Not Started", new User(), new User(), new Team(),_userC));
            

            return changeList;
        }

        [Test]
        public void calculateUserBookedDaysUserATest()
        {
            var changeList = generateTestChanges();
            ScheduleService scheduleService = new ScheduleService();
            scheduleService.scheduleChanges(changeList);
            int[] userPlannedDays = new int[3];

            for (int i = 0; i < _team.teamMembers.Count(); i++)
            {
                userPlannedDays[i] = (scheduleService.calculateUserBookedDays(changeList, _team.teamMembers.ElementAt(i).user.userID));
            }
            
            Assert.AreEqual(5, userPlannedDays[0]);
            Assert.AreEqual(6, userPlannedDays[1]);
            Assert.AreEqual(6, userPlannedDays[2]);
        }
    }
}