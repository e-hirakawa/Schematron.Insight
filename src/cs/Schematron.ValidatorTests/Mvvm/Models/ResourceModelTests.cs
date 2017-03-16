using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schematron.Validator.Mvvm.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.Validator.Mvvm.Models.Tests
{
    [TestClass()]
    public class ResourceModelTests
    {
        [TestMethod()]
        public void ResourceModelTest()
        {
            ResourceModel model = new ResourceModel();
            model.FullPath = @"C:\Users\ehirakawa\Documents\pv.develop\schematron\Schematron.Insight\src\cs\Schematron.ValidatorTests\Mvvm\Models\ResourceModelTests.cs";
            model.FullPath = @"go*e\dafa.cc";
            model.FullPath = null;
        }

    }
}