using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBOPerator.Biz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DBOPerator.Biz.Tests
{
    [TestClass()]
    public class BConStringTests
    {
        [TestMethod()]
        public void AddConStringTest()
        {
            try
            {
                var res = new BConString().AddConString("server=192.168.10.24;user id=root;password=testadmin");
                Thread.Sleep(2000);
            }
            catch (Exception)
            {

            }
        }


    }
}

