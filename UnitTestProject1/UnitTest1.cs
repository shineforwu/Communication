using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void UnitTestInit()
        {

        }
        [TestMethod]
        public void TestMethod1()
        {
            ushort[] dat = { 65535, 65535 };
            int start = 0;
            if (start < 0 || start + 1 >= dat.Length)
            {
                throw new Exception($"ushortToInt32索引超范围{start}");
            }
            byte[] tmp = new byte[4];
            byte[] byteH = BitConverter.GetBytes(dat[start + 1]);
            byte[] byteL = BitConverter.GetBytes(dat[start + 0]);
            tmp[0] = byteH[0];
            tmp[1] = byteH[1];
            tmp[2] = byteL[0];
            tmp[3] = byteL[1];

            int result = BitConverter.ToInt32(tmp, 0);
            Assert.IsTrue(1 == 1);
            int result2 = BitConverter.ToInt32(BitConverter.GetBytes(dat[0]).Concat(BitConverter.GetBytes(dat[1])).ToArray(), 0);
            Assert.IsTrue(1 == 1);
        }

        [TestMethod]
        public void TestMethod2()
        {
            ushort lowOrderValue = 65436;

            ushort highOrderValue = 65535;
            int result = BitConverter.ToInt32(BitConverter.GetBytes(lowOrderValue).Concat(BitConverter.GetBytes(highOrderValue)).ToArray(), 0);
            Assert.IsTrue(1 == 1);

        }
        [TestMethod]
        public void TestMethod3()
        {
            int a = -22;
            byte[] tmp = new byte[4];
            tmp=BitConverter.GetBytes(a);
            ushort lowOrderValue = BitConverter.ToUInt16(tmp,0);
            ushort highOrderValue = BitConverter.ToUInt16(tmp, 2);
            int result = BitConverter.ToInt32(BitConverter.GetBytes(lowOrderValue).Concat(BitConverter.GetBytes(highOrderValue)).ToArray(), 0);
            Assert.IsTrue(1 == 1);

        }
    }

}
