using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schematron.Validator.Mvvm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.Validator.Mvvm.Models.Tests
{
    [TestClass()]
    public class ProgressModelTests
    {
        [TestMethod()]
        public void SetValueTest()
        {
            ProgressModel progress = new ProgressModel();
            progress.SetValue(0, 0);
            Assert.AreEqual(0, progress.Value);
            progress.SetValue(1, 0);
            Assert.AreEqual(0, progress.Value);
            progress.SetValue(1, 10);
            Assert.AreEqual(10, progress.Value);
            progress.SetValue(10, 10);
            Assert.AreEqual(100, progress.Value);
        }
    }
}