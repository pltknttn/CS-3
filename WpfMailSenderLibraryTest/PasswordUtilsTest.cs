using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfMailSenderScheduler.Data;

namespace WpfMailSenderLibraryTest
{
    [TestClass]
    public class PasswordUtilsTest
    {
        private string testStr;

        [TestInitialize]
        public void TestPrepare()
        {
            testStr = "abcd";
        } 

        [TestMethod]
        public void TestDecodePassword_Successful()
        { 
            const string expectedDecodePassword = "bcde";
            var result = PasswordUtils.Decode(testStr);
            Assert.AreEqual(expectedDecodePassword, result);
        }

        [TestMethod]
        public void TestDecodePassword_Error()
        { 
            const string expectedDecodePassword = "dcda";
            var result = PasswordUtils.Decode(testStr);
            Assert.AreEqual(expectedDecodePassword, result);
        }

        [TestMethod]
        public void TestDecodePassword_Empty()
        { 
            const string expectedDecodePassword = "";
            var result = PasswordUtils.Decode(testStr);
            Assert.AreEqual(expectedDecodePassword, result);
        }

        [TestMethod]
        public void TestEncodePassword_Successful()
        { 
            const string expectedDecodePassword = "`abc";
            var result = PasswordUtils.Encode(testStr);
            Assert.AreEqual(expectedDecodePassword,result);
        }

        [TestMethod]
        public void TestEncodPassword_Error()
        {
            const string expectedDecodePassword = "abca";
            var result = PasswordUtils.Encode(testStr);
            Assert.AreEqual(expectedDecodePassword, result);
        }
    }
}
