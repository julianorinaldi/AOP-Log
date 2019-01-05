using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AOPLibrary
{
    public class Example2
    {

        public Example2()
        {
            Method1();
            Method2();
        }

        private void Method1()
        {
            Thread.Sleep(1000);
        }

        private void Method2()
        {
            Thread.Sleep(1000);
        }
    }
}
