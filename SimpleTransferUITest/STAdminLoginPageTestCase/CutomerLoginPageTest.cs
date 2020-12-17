using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SimpleTransferUITest.BaseTest;
using SimpleTransferUITest.STAdminLoginPage;

namespace SimpleTransferUITest.STAdminLoginPageTestCase
{
    class CutomerLoginPageTest : TestBase
    {
        CustomerLoginPage logpage;

        [OneTimeSetUp]
        public void OpenApplication()
        {
            LaunchBrowseradmin();
            logpage = new CustomerLoginPage(_driver);

        }
        [Test, Order(1)]
        public void TestValidLogin()
        {
            string s1 = "anshuvesuviouk@yopmail.com";
            string s2 = "Test@12";
            logpage.ValidLogin(s1, s2);
            Console.WriteLine(_driver.Title);
        }

        [Test, Order(2)]
        public void LogOutTest()
        {
            logpage.LogOutfromApplication();
            CloseBrowser();
        }
    }
}
