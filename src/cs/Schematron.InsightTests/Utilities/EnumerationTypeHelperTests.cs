using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schematron.Insight.Validation;

namespace Schematron.Insight.Utilities.Tests
{
    [TestClass()]
    public class EnumerationTypeHelperTests
    {
        [TestMethod()]
        public void ContainsTest()
        {
            ResultStatus status = ResultStatus.Assert | ResultStatus.Report;
            Assert.AreEqual(true, status.Contains(ResultStatus.Assert));
            Assert.AreEqual(false, status.Contains(ResultStatus.SyntaxError));
            Assert.AreEqual(true, status.Contains(ResultStatus.None));
            Assert.AreEqual(true, status.Contains(ResultStatus.Report));
        }

        [TestMethod()]
        public void AppendTest()
        {
            ResultStatus status = ResultStatus.None;
            status = status.Append(ResultStatus.Assert);
            Assert.AreEqual(ResultStatus.Assert, status);
            status = status.Append(ResultStatus.Report);
            Assert.AreEqual(ResultStatus.Assert | ResultStatus.Report, status);
            status = status.Append(ResultStatus.SyntaxError);
            Assert.AreEqual(ResultStatus.Assert | ResultStatus.Report | ResultStatus.SyntaxError, status);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            ResultStatus status = ResultStatus.Assert | ResultStatus.Report | ResultStatus.SyntaxError;
            Assert.AreEqual(ResultStatus.Assert | ResultStatus.Report | ResultStatus.SyntaxError, status);
            status = status.Remove(ResultStatus.Assert);
            Assert.AreEqual(ResultStatus.Report | ResultStatus.SyntaxError, status);
            status = status.Remove(ResultStatus.Report);
            Assert.AreEqual(ResultStatus.SyntaxError, status);
            status = status.Remove(ResultStatus.SyntaxError);
            Assert.AreEqual(ResultStatus.None, status);
        }

        [TestMethod()]
        public void DisplayNameTest()
        {
            Assert.AreEqual(ResultStatus.Assert.DisplayName(), "Assert");
            Assert.AreEqual(ResultStatus.Report.DisplayName(), "Report");
            Assert.AreEqual(ResultStatus.SyntaxError.DisplayName(), "Syntax Error");
            Assert.AreEqual(ResultStatus.None.DisplayName(), "None");
        }

        [TestMethod()]
        public void GetValueFromDisplayNameTest()
        {
            Assert.AreEqual(EnumerationTypeHelper.GetValueFromDisplayName<ResultStatus>("Assert"), ResultStatus.Assert);
            Assert.AreEqual(EnumerationTypeHelper.GetValueFromDisplayName<ResultStatus>("Report"), ResultStatus.Report);
            Assert.AreEqual(EnumerationTypeHelper.GetValueFromDisplayName<ResultStatus>("Syntax Error"), ResultStatus.SyntaxError);
            Assert.AreEqual(EnumerationTypeHelper.GetValueFromDisplayName<ResultStatus>("None"), ResultStatus.None);
        }
    }
}