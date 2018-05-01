using NUnit.Framework;
using SirmaSolutions.EmployeesTool.BLL.Tests.Proxies;
using System;

namespace SirmaSolutions.EmployeesTool.BLL.Tests.TextParsers
{
    [TestFixture]
    public class JobHistoryListParserTests
    {
        private JobHistoryListParserProxy _proxy;
        private string dateTimeFormat = "yyyy-MM-dd";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _proxy = new JobHistoryListParserProxy();
        }

        [Test]
        public void InvalidEmployeeId()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            {
                _proxy.ParseLineTest("asd, 1, 2013-11-01, 2015-11-01", dateTimeFormat);
            });

            Assert.AreEqual("Invalid employee id.", exception.Message);
        }

        [Test]
        public void InvalidProjectId()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            {
                _proxy.ParseLineTest("1, asd, 2013-11-01, 2015-11-01", dateTimeFormat);
            });

            Assert.AreEqual("Invalid project id.", exception.Message);
        }

        [Test]
        public void InvalidDateFrom()
        {
            FormatException exception = Assert.Throws<FormatException>(() =>
            {
                _proxy.ParseLineTest("1, 1, 2013-11.01, 2015-11-01", dateTimeFormat);
            });

            Assert.AreEqual("Invalid date from.", exception.Message);
        }

        [Test]
        public void InvalidDateTo()
        {
            FormatException exception = Assert.Throws<FormatException>(() =>
            {
                _proxy.ParseLineTest("1, 1, 2013-11-01, 2015-11.01", dateTimeFormat);
            });

            Assert.AreEqual("Invalid date to.", exception.Message);
        }
    }
}
