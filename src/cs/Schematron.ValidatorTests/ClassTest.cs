﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.ValidatorTests
{
    [TestClass()]
    public class ClassTest
    {
        [TestMethod()]
        public void Test()
        {
            Class1 c11 = new Class1();
            c11.Update();
        }
    }
}
