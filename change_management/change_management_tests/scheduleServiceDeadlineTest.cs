using NUnit.Framework;
using change_management.Services;
using change_management.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Tests
{
    public class ScheduleServiceDeadlineTest
    {
        private ScheduleService _scheduleService;

        [SetUp]
        public void SetUp()
        {
            _scheduleService = new ScheduleService();
            SessionService.loggedInUser = new User(1, "Test", "User", "Tester");
        }

        private List<Change> generateTestData() {
            var changeList = new List<Change>();

            changeList.Add(new Change(1, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-5), DateTime.Now, "In progress", new User(), new User(), new Team(), SessionService.loggedInUser));
            changeList.Add(new Change(2, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-4), null, "Not Started", new User(), new User(), new Team(), SessionService.loggedInUser));
            changeList.Add(new Change(3, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(25), 2, 6, DateTime.Now.AddDays(-3), DateTime.Now, "In progress", new User(), new User(), new Team(), SessionService.loggedInUser));
            changeList.Add(new Change(4, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(14), 1, 4, DateTime.Now.AddDays(-2), null, "Not Started", new User(), new User(), new Team(), SessionService.loggedInUser));
            changeList.Add(new Change(5, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(12), 2, 6, DateTime.Now.AddDays(-5), DateTime.Now, "In progress", new User(), new User(), new Team(), SessionService.loggedInUser));
            changeList.Add(new Change(6, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-5), null, "Not Started", new User(), new User(), new Team(), SessionService.loggedInUser));
            changeList.Add(new Change(7, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-4), DateTime.Now, "In progress", new User(), new User(), new Team(), SessionService.loggedInUser));
            changeList.Add(new Change(8, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(25), 2, 6, DateTime.Now.AddDays(-3), null, "Not Started", new User(), new User(), new Team(), SessionService.loggedInUser));
            changeList.Add(new Change(9, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(14), 1, 4, DateTime.Now.AddDays(-2), DateTime.Now, "In progress", new User(), new User(), new Team(), SessionService.loggedInUser));
            changeList.Add(new Change(10, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(12), 2, 6, DateTime.Now.AddDays(-5), null,"Not Started", new User(), new User(), new Team(), SessionService.loggedInUser));

            return changeList;
        }

        [Test]
        public void calculateUserBookedDaysTest()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            int userPlannedDays = scheduleService.calculateUserBookedDays(changeList);

            Assert.AreEqual(22, userPlannedDays);
        }

        [Test]
        public void deadlineStatusSetterTest()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            int userPlannedDays = scheduleService.calculateUserBookedDays(changeList);

            Assert.AreEqual(22, userPlannedDays);
        }
    }
}