using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using SirmaSolutions.EmployeesTool.BLL.Entities;
using SirmaSolutions.EmployeesTool.BLL.Selectors;

namespace SirmaSolutions.EmployeesTool.BLL.Tests.Selectors
{
    [TestFixture]
    public class CommonProjectsCouplesSelectorTests
    {
        private CommonProjectsCouplesSelector _selector;

        private DateTime GetDate(string date)
        {
            return DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _selector = new CommonProjectsCouplesSelector();
        }

        [Test]
        public void NoOverlapingProjectsNoCouple()
        {
            List<JobHistory> jobHistories = new List<JobHistory>();

            jobHistories.Add(new JobHistory(1, 1, GetDate("11.02.2017"), GetDate("15.03.2017")));
            jobHistories.Add(new JobHistory(2, 2, GetDate("11.02.2017"), GetDate("15.03.2017")));

            List<CommonProjectsResult> results = _selector.Select(jobHistories);

            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void NoOverlapingDatesNoCouple()
        {
            List<JobHistory> jobHistories = new List<JobHistory>();

            jobHistories.Add(new JobHistory(1, 1, GetDate("11.02.2017"), GetDate("15.03.2017")));
            jobHistories.Add(new JobHistory(2, 1, GetDate("11.02.2018"), GetDate("15.03.2018")));

            List<CommonProjectsResult> results = _selector.Select(jobHistories);

            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void OverlapingDatesNotFullyCouple()
        {
            List<JobHistory> jobHistories = new List<JobHistory>();

            jobHistories.Add(new JobHistory(1, 1, GetDate("11.02.2017"), GetDate("15.03.2017")));
            jobHistories.Add(new JobHistory(2, 1, GetDate("09.02.2017"), GetDate("20.02.2017")));

            List<CommonProjectsResult> results = _selector.Select(jobHistories);

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(10, results[0].Days);
        }

        [Test]
        public void OverlapingDatesFullyCouple()
        {
            List<JobHistory> jobHistories = new List<JobHistory>();

            jobHistories.Add(new JobHistory(1, 1, GetDate("11.02.2017"), GetDate("15.03.2017")));
            jobHistories.Add(new JobHistory(2, 1, GetDate("15.02.2017"), GetDate("24.02.2017")));

            List<CommonProjectsResult> results = _selector.Select(jobHistories);

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(10, results[0].Days);
        }
    }
}
