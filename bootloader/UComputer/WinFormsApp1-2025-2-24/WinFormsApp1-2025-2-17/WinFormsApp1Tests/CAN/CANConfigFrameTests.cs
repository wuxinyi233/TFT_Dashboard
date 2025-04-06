using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinFormsApp1.CAN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Tests
{
    [TestClass()]
    public class CANConfigFrameTests
    {
        [TestMethod()]
        public void ToBytesTest()
        {
            //Assert.Fail();
            ICANFrame configFrame = new CANConfigFrame();
            var bytes = configFrame.ToBytes();
            var str = configFrame.ToHexBytes();
        }
    }
}