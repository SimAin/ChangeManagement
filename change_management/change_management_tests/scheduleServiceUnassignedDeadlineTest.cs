using NUnit.Framework;
using change_management.Services;
using change_management.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Tests
{
    public class scheduleServiceUnassignedDeadlineTest
    {
        private ScheduleService _scheduleService; 
        private Team _team = new Team(1, "newTeam", 15 );
        private User _user2 = new User(5, "Test2", "User2", "Dev");

        [SetUp]
        public void SetUp()
        {
            _scheduleService = new ScheduleService();
            SessionService.loggedInUser = new User(1, "Test", "User", "Tester");

            _team.teamMembers = new List<TeamMember>(){
                new TeamMember(SessionService.loggedInUser, 5),
                new TeamMember(_user2, 5),
            };

            SessionService.loggedInTeam = _team;
        }

        private List<Change> generateTestData() {
            var changeList = new List<Change>();

            changeList.Add(new Change(1, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-5), DateTime.Now, "In progress", new User(), new User(), new Team(), SessionService.loggedInUser));
            changeList.Add(new Change(2, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(46), 3, 3, DateTime.Now.AddDays(-4), null, "Not Started", new User(), new User(), new Team(), SessionService.loggedInUser));
            changeList.Add(new Change(3, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(2), 2, 6, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-4), "In progress", new User(), new User(), new Team(), _user2));
            changeList.Add(new Change(4, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(14), 1, 4, DateTime.Now.AddDays(-2), null, "Not Started", new User(), new User(), new Team(), _user2));
            changeList.Add(new Change(5, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(5), 2, 6, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-2), "In progress", new User(), new User(), new Team(), new User(0, "", "", "")));
            changeList.Add(new Change(6, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-5), null, "Not Started", new User(), new User(), new Team(), new User(0, "", "", "")));
            changeList.Add(new Change(7, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(6), 3, 9, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-2), "In progress", new User(), new User(), new Team(), new User(0, "", "", "")));
            changeList.Add(new Change(8, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(25), 2, 6, DateTime.Now.AddDays(-3), null, "Not Started", new User(), new User(), new Team(), new User(0, "", "", "")));
            changeList.Add(new Change(9, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(1), 1, 7, DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-2), "In progress", new User(), new User(), new Team(), new User(0, "", "", "")));
            changeList.Add(new Change(10, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(12), 2, 6, DateTime.Now.AddDays(-5), null,"Not Started", new User(), new User(), new Team(), new User(0, "", "", "")));

            return changeList;
        }

        [Test]
        public void calculateUserLowestDaysTest()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            scheduleService.calculateDeadlineStatus(changeList);

            scheduleService.calculateUserBookedDays(changeList);
            scheduleService.calculateUserPlannedDays(changeList);
            var member = scheduleService.calculateTeamMemberWithLeastPlanned(new Change(6, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-5), null, "Not Started", new User(), new User(), new Team(), new User(0, "", "", "")));
            
            Assert.AreEqual(9, member.userLowestDays );
        }

        [Test]
        public void calculateUserLowestDays2Test()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();

            List<TeamMember> members = SessionService.loggedInTeam.teamMembers.ToList();

            scheduleService.calculateUserBookedDays(changeList);
            scheduleService.calculateUserPlannedDays(changeList);
            scheduleService.calculateTeamMemberWithLeastPlanned(new Change(6, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 17, DateTime.Now.AddDays(-5), null, "Not Started", new User(), new User(), new Team(), new User(0, "", "", "")));
            var member = scheduleService.calculateTeamMemberWithLeastPlanned(new Change(6, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 5, DateTime.Now.AddDays(-5), null, "Not Started", new User(), new User(), new Team(), new User(0, "", "", "")));

            Assert.AreEqual(11, member.userLowestDays );
        }
        
        [Test]
        public void deadlineInProgressStatus1SetterTest () {
            var changeList = generateTestData();
            int daysRemaining = changeList[1].processingTime - ((int) (changeList[1].startedDate ?? DateTime.Now).Subtract (DateTime.Now).TotalDays);
            ScheduleService scheduleService = new ScheduleService ();
            scheduleService.deadlineStatusSetter (changeList[1], daysRemaining);

            Assert.AreEqual (1, changeList[1].deadlineStatus);
        }

        [Test]
        public void deadlineInProgressStatus2SetterTest () {
            var changeList = generateTestData();
            int daysRemaining = changeList[4].processingTime - Math.Abs(((int) (changeList[4].startedDate ?? DateTime.Now).Subtract (DateTime.Now).TotalDays));
            ScheduleService scheduleService = new ScheduleService ();
            scheduleService.scheduleChanges(changeList);
            scheduleService.deadlineStatusSetter (changeList[4], daysRemaining);

            Assert.AreEqual (2, changeList[4].deadlineStatus);
        }

        [Test]
        public void deadlineInProgressStatus3SetterTest () {
            var changeList = generateTestData();
            int daysRemaining = changeList[2].processingTime - Math.Abs(((int) (changeList[2].startedDate ?? DateTime.Now).Subtract (DateTime.Now).TotalDays));
            ScheduleService scheduleService = new ScheduleService ();
            scheduleService.scheduleChanges(changeList);
            scheduleService.deadlineStatusSetter (changeList[2], daysRemaining);

            Assert.AreEqual (3, changeList[2].deadlineStatus);
        }

        [Test]
        public void deadlineInProgressStatus4SetterTest () {
            var changeList = generateTestData();
            int daysRemaining = changeList[6].processingTime - Math.Abs(((int) (changeList[6].startedDate ?? DateTime.Now).Subtract (DateTime.Now).TotalDays));
            ScheduleService scheduleService = new ScheduleService ();
            scheduleService.scheduleChanges(changeList);
            scheduleService.deadlineStatusSetter (changeList[6], daysRemaining);

            Assert.AreEqual (4, changeList[6].deadlineStatus);
        }

        [Test]
        public void deadlineInProgressStatus5SetterTest () {
            var changeList = generateTestData();
            int daysRemaining = changeList[8].processingTime - Math.Abs(((int) (changeList[8].startedDate ?? DateTime.Now).Subtract (DateTime.Now).TotalDays));
            ScheduleService scheduleService = new ScheduleService ();
            scheduleService.scheduleChanges(changeList);
            scheduleService.deadlineStatusSetter (changeList[8], daysRemaining);

            Assert.AreEqual (5, changeList[8].deadlineStatus);
        }

        [Test]
        public void calculateDeadlineStatusInProgress()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();

            var changes = scheduleService.calculateDeadlineStatus(changeList);

            Assert.AreEqual(1, changes[1].deadlineStatus);
        }

        [Test]
        public void calculateDeadlineStatusNotStarted()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();

            var changes = scheduleService.calculateDeadlineStatus(changeList);

            Assert.AreEqual(1, changes[0].deadlineStatus);
        }
    }
}