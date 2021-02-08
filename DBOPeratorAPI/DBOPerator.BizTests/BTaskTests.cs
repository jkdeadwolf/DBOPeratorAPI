using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBOPerator.Biz;
using System;
using System.Collections.Generic;
using System.Text;
using DBOPerator.Model;

namespace DBOPerator.Biz.Tests
{
    [TestClass()]
    public class BTaskTests
    {
        [TestMethod()]
        public void AddDBTaskTest()
        {
            try
            {
                var task = new DBTask()
                {
                    BusinessKeyID = "202102071129559905157225",
                    BusinessType = BusinessType.表建表,

                };
                var res = new BTask().AddDBTask(task);
            }
            catch (Exception)
            {
            }
        }
    }
}