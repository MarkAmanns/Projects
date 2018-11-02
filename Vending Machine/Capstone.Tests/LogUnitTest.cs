using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Tests
{
    [TestClass]
    public class  LogUnitTest
    {
        [TestMethod]
        public void LogTest()
        {
            Log log = new Log();
            Assert.AreEqual(true, log.WriteLog("Log Test: Change", 5.55M, 0.00M), "Expected true as returned from Log method.");
 
            // view test transaction added to log at C:\Workspace\team\week-4-pair-exercises-team-1\c#-capstone\etc\Log.txt
        }
    }
}
