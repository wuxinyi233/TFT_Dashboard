using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinFormsApp1.CAN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.CAN.Extension;

namespace WinFormsApp1.CAN.Tests
{
    [TestClass()]
    public class CANDataFrameTests
    {
        [TestMethod()]
        public void ToBytesTest()
        {
            //Assert.Fail();
            var dataFrame = new CANDataFrame();
            dataFrame.ID = 0x1314;
            dataFrame.Data = [1, 2, 3, 4, 5, 6, 7, 8];
            var bytes = dataFrame.ToBytes();
            var str = dataFrame.ToHexBytes();
        }

        [TestMethod()]
        public void FrameCheckTest()
        {
            var dataFrame = new CANDataFrame();
            dataFrame.FrameCheck("AA 55 01 02 01 01 00 00 00 08 00 00 00 00 00 00 00 00 00 0D".ToHexByteArray());
        }
    }
}