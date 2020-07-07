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
            //(int id, SystemEntity sys, string t, string desc, bool critical, DateTime due, int pri, int processing time, User responsible)
            changeList.Add(new Change(1, new SystemEntity(), "type", "description", true, new DateTime(24/07/2020), 3, 3, new User()));
            changeList.Add(new Change(2, new SystemEntity(), "type", "description", true, new DateTime(23/07/2020), 3, 3, new User()));
            changeList.Add(new Change(3, new SystemEntity(), "type", "description", true, new DateTime(25/07/2020), 2, 6, new User()));
            changeList.Add(new Change(4, new SystemEntity(), "type", "description", true, new DateTime(14/07/2020), 1, 4, new User()));
            changeList.Add(new Change(5, new SystemEntity(), "type", "description", true, new DateTime(12/07/2020), 2, 6, new User()));
            changeList.Add(new Change(6, new SystemEntity(), "type", "description", true, new DateTime(24/07/2020), 3, 3, new User()));
            changeList.Add(new Change(7, new SystemEntity(), "type", "description", true, new DateTime(23/07/2020), 3, 3, new User()));
            changeList.Add(new Change(8, new SystemEntity(), "type", "description", true, new DateTime(25/07/2020), 2, 6, new User()));
            changeList.Add(new Change(9, new SystemEntity(), "type", "description", true, new DateTime(14/07/2020), 1, 4, new User()));
            changeList.Add(new Change(10, new SystemEntity(), "type", "description", true, new DateTime(12/07/2020), 2, 6, new User()));

            return changeList;
        }

        [Test]
        public void scheduleChangesCorrectOrder()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            List<Change> orderedChanges = scheduleService.scheduleChanges(changeList).ToList();

            var changeList2 = generateTestData();

            //CollectionAssert.AreEqual( changeList[0].changeId, changeList2[0].changeId );
            // Assert.That(changeList, Has.Exactly(1).EqualTo(generateTestData()));
            Assert.AreEqual( orderedChanges[0].changeId, changeList2[0].changeId);
            Assert.AreEqual( orderedChanges[1].changeId, changeList2[1].changeId);
            Assert.AreEqual( orderedChanges[2].changeId, changeList2[2].changeId);
            Assert.AreEqual( orderedChanges[3].changeId, changeList2[3].changeId);
            Assert.AreEqual( orderedChanges[4].changeId, changeList2[4].changeId);
            Assert.AreEqual( orderedChanges[5].changeId, changeList2[5].changeId);
            Assert.AreEqual( orderedChanges[6].changeId, changeList2[6].changeId);
            Assert.AreEqual( orderedChanges[7].changeId, changeList2[7].changeId);
            Assert.AreEqual( orderedChanges[8].changeId, changeList2[8].changeId);
            Assert.AreEqual( orderedChanges[9].changeId, changeList2[9].changeId);

        }
    }
}