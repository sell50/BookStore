using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        UserData userData = new UserData();
        string inputName, inputPassword;
        int actualUserId;
        [TestMethod]
        public void TestMethod1()
        {
            // specify the value of test inout
            inputName = "amula";
            inputPassword = "am1234";
            // specify  the value of expected outputs
            Boolean expectedReturn = true;
            int expectedUserId = 1;
            //Obtain the actual outputs by calling the method under testing
            Boolean actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;
            //Verify the result:
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);


        }
        [TestMethod]
        public void TestMethod2()
        {
            //UserName input was left blank, but password was filled, The expected output would be -1
            inputName = " ";
            inputPassword = "dc1234";
            // specify  the value of expected outputs
            Boolean expectedReturn = false;
            int expectedUserId = -1;
            //Obtain the actual outputs by calling the method under testing
            Boolean actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;
            //Verify the result:
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);


        }
        [TestMethod]
        public void TestMethod3()
        {
            //Password input was left blank, but password was filled, The expected output would be -1
            inputName = "amula";
            inputPassword = " ";
            // specify  the value of expected outputs
            Boolean expectedReturn = false;
            int expectedUserId = -1;
            //Obtain the actual outputs by calling the method under testing
            Boolean actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;
            //Verify the result:
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);


        }
        [TestMethod]
        public void TestMethod4()
        {
            //Password starts with a number, The expected output would be -1
            inputName = "amula";
            inputPassword = "1234ab";
            // specify  the value of expected outputs
            Boolean expectedReturn = false;
            int expectedUserId = -1;
            //Obtain the actual outputs by calling the method under testing
            Boolean actualReturn = userData.LogIn(inputName, inputPassword);
            actualUserId = userData.UserID;
            //Verify the result:
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);


        }
    }
}