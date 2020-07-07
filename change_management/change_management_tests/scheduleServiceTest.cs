using NUnit.Framework;
using change_management.Services;
using change_management.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Tests
{
    public class ScheduleServiceTest
    {
        private ScheduleService _scheduleService;

        [SetUp]
        public void SetUp()
        {
            _scheduleService = new ScheduleService();
        }

        private List<Change> generateTestData() {
            var changeList = new List<Change>();

            changeList.Add(new Change(1, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-5), DateTime.Now, "In progress", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(2, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-4), null, "Not Started", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(3, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(25), 2, 6, DateTime.Now.AddDays(-3), DateTime.Now, "In progress", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(4, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(14), 1, 4, DateTime.Now.AddDays(-2), null, "Not Started", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(5, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(12), 2, 6, DateTime.Now.AddDays(-5), DateTime.Now, "In progress", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(6, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-5), null, "Not Started", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(7, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 3, DateTime.Now.AddDays(-4), DateTime.Now, "In progress", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(8, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(25), 2, 6, DateTime.Now.AddDays(-3), null, "Not Started", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(9, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(14), 1, 4, DateTime.Now.AddDays(-2), DateTime.Now, "In progress", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(10, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(12), 2, 6, DateTime.Now.AddDays(-5), null,"Not Started", new User(), new User(), new Team(), new User()));

            return changeList;
        }

        [Test]
        public void scheduleChangeOrderCriticalFirst()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            List<Change> orderedChanges = scheduleService.scheduleChanges(changeList).ToList();

            Assert.AreEqual(true, orderedChanges[0].criticality);
            Assert.AreEqual(true, orderedChanges[1].criticality);
            Assert.AreEqual(true, orderedChanges[2].criticality);
            Assert.AreEqual(true, orderedChanges[3].criticality);
            Assert.AreEqual(true, orderedChanges[4].criticality);
        }

        [Test]
        public void scheduleChangeOrderNonCriticalLast()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            List<Change> orderedChanges = scheduleService.scheduleChanges(changeList).ToList();
       
            Assert.AreEqual(false, orderedChanges[5].criticality);
            Assert.AreEqual(false, orderedChanges[6].criticality);
            Assert.AreEqual(false, orderedChanges[7].criticality);
            Assert.AreEqual(false, orderedChanges[8].criticality);
            Assert.AreEqual(false, orderedChanges[9].criticality);
        }

        [Test]
        public void scheduleChangeOrderCriticalLowestLaxity()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            List<Change> orderedChanges = scheduleService.scheduleChanges(changeList).ToList();

            Assert.AreEqual(-17, orderedChanges[0].laxity);
            Assert.AreEqual(-17, orderedChanges[1].laxity);
            Assert.AreEqual(-26, orderedChanges[2].laxity);
            Assert.AreEqual(-26, orderedChanges[3].laxity);
            Assert.AreEqual(-30, orderedChanges[4].laxity);
        }

        [Test]
        public void scheduleChangeOrderNonCriticalLowestLaxity()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            List<Change> orderedChanges = scheduleService.scheduleChanges(changeList).ToList();

            Assert.AreEqual(-17, orderedChanges[5].laxity);
            Assert.AreEqual(-17, orderedChanges[6].laxity);
            Assert.AreEqual(-26, orderedChanges[7].laxity);
            Assert.AreEqual(-26, orderedChanges[8].laxity);
            Assert.AreEqual(-30, orderedChanges[9].laxity);
        }

        [Test]
        public void scheduleChangeOrderCriticalHighestPriority()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            List<Change> orderedChanges = scheduleService.scheduleChanges(changeList).ToList();

            Assert.AreEqual(1, orderedChanges[0].priority);
            Assert.AreEqual(2, orderedChanges[1].priority);
            Assert.AreEqual(3, orderedChanges[2].priority);
            Assert.AreEqual(3, orderedChanges[3].priority);
        }

        [Test]
        public void scheduleChangeOrderNonCriticalHighestPriority()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            List<Change> orderedChanges = scheduleService.scheduleChanges(changeList).ToList();

            Assert.AreEqual(1, orderedChanges[5].priority);
            Assert.AreEqual(2, orderedChanges[6].priority);
            Assert.AreEqual(3, orderedChanges[7].priority);
            Assert.AreEqual(3, orderedChanges[8].priority);
        }

        [Test]
        public void scheduleChangeOrderCriticalFirstCome()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            List<Change> orderedChanges = scheduleService.scheduleChanges(changeList).ToList();

            Assert.AreEqual(DateTime.Now.AddDays(-4).Date, orderedChanges[2].createdDate.Date);
            Assert.AreEqual(DateTime.Now.AddDays(-5).Date, orderedChanges[3].createdDate.Date);
        }

        [Test]
        public void scheduleChangeOrderNonCriticalFirstCome()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            List<Change> orderedChanges = scheduleService.scheduleChanges(changeList).ToList();

            Assert.AreEqual(DateTime.Now.AddDays(-4).Date, orderedChanges[7].createdDate.Date);
            Assert.AreEqual(DateTime.Now.AddDays(-5).Date, orderedChanges[8].createdDate.Date);
        }
    }
}