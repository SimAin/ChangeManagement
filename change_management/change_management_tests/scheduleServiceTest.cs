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
                                  //int id, SystemEntity sys, string t, string desc, bool critical, DateTime due, int pri, int pt, DateTime create, DateTime? start ,string state, User app, User stake, Team team, User responsible
            changeList.Add(new Change(1, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(24), 3, 3, DateTime.Now, new DateTime(07/07/2020), "In progress", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(2, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(24), 3, 3, DateTime.Now, null, "Not Started", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(3, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(25), 2, 6, DateTime.Now, DateTime.Now.AddDays(2), "In progress", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(4, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(14), 1, 4, DateTime.Now, null, "Not Started", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(5, new SystemEntity(), "type", "description", true, DateTime.Now.AddDays(12), 2, 6, DateTime.Now, DateTime.Now.AddDays(2), "In progress", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(6, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 3, DateTime.Now, null, "Not Started", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(7, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(24), 3, 3, DateTime.Now, DateTime.Now.AddDays(2), "In progress", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(8, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(25), 2, 6, DateTime.Now, null, "Not Started", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(9, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(14), 1, 4, DateTime.Now, DateTime.Now.AddDays(2), "In progress", new User(), new User(), new Team(), new User()));
            changeList.Add(new Change(10, new SystemEntity(), "type", "description", false, DateTime.Now.AddDays(12), 2, 6, DateTime.Now, null,"Not Started", new User(), new User(), new Team(), new User()));

            return changeList;
        }

        [Test]
        public void scheduleChangesCorrectOrder()
        {
            var changeList = generateTestData();
            ScheduleService scheduleService = new ScheduleService();
            List<Change> orderedChanges = scheduleService.scheduleChanges(changeList).ToList();

            var changeList2 = generateTestData();

            foreach (var t in changeList)
            {
                Console.WriteLine(t.changeId);
            }

            //CollectionAssert.AreEqual( changeList[0].changeId, changeList2[0].changeId );
            // Assert.That(changeList, Has.Exactly(1).EqualTo(generateTestData()));
            Assert.AreEqual(5, orderedChanges[0].changeId);
            Assert.AreEqual(4, orderedChanges[1].changeId);
            Assert.AreEqual(2, orderedChanges[2].changeId);
            Assert.AreEqual(1, orderedChanges[3].changeId);
            Assert.AreEqual(3, orderedChanges[4].changeId);
            Assert.AreEqual(10, orderedChanges[5].changeId);
            Assert.AreEqual(9, orderedChanges[6].changeId);
            Assert.AreEqual(7, orderedChanges[7].changeId);
            Assert.AreEqual(6, orderedChanges[8].changeId);
            Assert.AreEqual(8, orderedChanges[9].changeId);

        }
    }
}