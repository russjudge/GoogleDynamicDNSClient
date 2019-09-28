using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoogleDynamicDNSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSLibrary.Tests
{
    [TestClass()]
    public class ProcessTests
    {
        [TestMethod()]
        public void GetPublicIPTest()
        {
            string expectedResult = "2600:1700:1e60:1fe0::455";
            string actualResult = GoogleDynamicDNSLibrary.Process.GetPublicIP();

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}