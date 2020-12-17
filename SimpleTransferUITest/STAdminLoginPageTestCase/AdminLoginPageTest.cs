using NUnit.Framework;
using SimpleTransferUITest.BaseTest;
using SimpleTransferUITest.STAdminLoginPage;
using System;

namespace SimpleTransferUITest.STAdminLoginPageTestCase
{
    public class AdminLoginPageTest : TestBase
    {
        AdminLoginPage loginpage;

        [OneTimeSetUp]
        public void OpenApplication()
        {
            LaunchBrowseradmin();
            loginpage = new AdminLoginPage(_driver);
        }

        [Test, Order(1)]
        public void TestValidLogin()
        {
            string s1 = "kishan@vesuviois.com";
            string s2 = "Abcd1#";
            loginpage.ValidLogin(s1, s2);
            Console.WriteLine(_driver.Title);
            //Console.WriteLine(_driver.Title);
        }

        [Test, Order(2)]
        public void LogOutTest()
        {
            loginpage.LogOutfromApplication();
            CloseBrowser();
        }
    }
}
