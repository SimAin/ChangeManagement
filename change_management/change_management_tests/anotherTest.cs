using NUnit.Framework;
using change_management.Services;
using change_management.Models;
using System.Collections.Generic;
using System;

namespace Tests
{
    public class anotherTest
    {
        private ScheduleService _scheduleService;

        [SetUp]
        public void SetUp()
        {
            _scheduleService = new ScheduleService();
        }

        private List<Change> generateTestData() {
            var changeList = new List<Change>();

            changeList.Add(new Change(1, new SystemEntity(), "a type", "a description", true, new DateTime(04/07/2020), 3, 3, new User()));
            return changeList;
        }

        [Test]
        public void scheduleChangesCorrectOrder()
        {
            var changeList = generateTestData();

            Assert.That(changeList, Has.Exactly(1).EqualTo(generateTestData()));
        }
    }
}