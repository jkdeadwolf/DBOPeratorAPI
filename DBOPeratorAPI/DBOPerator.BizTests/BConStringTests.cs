using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBOPerator.Biz;
using System;
using System.Collections.Generic;
using System.Text;

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
                var res = new BConString().AddConString("server=192.168.10.24;user id=root;password=testadmin;persistsecurityinfo=True;database=DBOPerator;charset=utf8;AllowUserVariables=True;SslMode=none");
            }
            catch (Exception)
            {

            }
        }


    }
}

