using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.ValidatorTests
{
    public class Class2:Class1
    {
        public void Update()
        {
            base.Update();
            Console.WriteLine("Class2");
        }
    }
}
