using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
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
            Debug.WriteLine("TestPrepare");
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
            Assert.AreEqual(expectedDecodePassword,result, "Корректный результат"); 
        }

        [TestMethod]
        public void TestEncodPassword_Error()
        {
            const string expectedDecodePassword = "abca";
            var result = PasswordUtils.Encode(testStr);
            Assert.AreEqual(expectedDecodePassword, result, "Значения не совпали"); 
        }

        [TestMethod]
        public void TestEncodPassword_Error_v2()
        {
            const string expectedDecodePassword = "abca";
            var result = PasswordUtils.Encode(testStr); 
            StringAssert.Contains(expectedDecodePassword, result); 
        }

        [TestCleanup]
        public void CleanUp()
        {
            Debug.WriteLine("Clenup");
        }
    }
}
