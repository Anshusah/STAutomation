using OpenQA.Selenium;
using System.Threading;

namespace SimpleTransferUITest.STAdminLoginPage
{
    public class AdminLoginPage
    {
        private IWebDriver _driver;

        public AdminLoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement TextUserName => _driver.FindElement(By.XPath("//input[@id='Email']"));
        public IWebElement TextPassword => _driver.FindElement(By.XPath("//input[@id='Password']"));
        public IWebElement LoginBtn => _driver.FindElement(By.XPath("//button[contains(text(),'Log In')]"));


        public IWebElement LogOut => _driver.FindElement(By.XPath("//div[@class='nav-item-topbar__user']"));
        public IWebElement LogOutOne => _driver.FindElement(By.XPath("//a[contains(text(),'Logout')]"));

        public void ValidLogin(string uName, string pwd)
        {
            TextUserName.SendKeys(uName);
            TextPassword.SendKeys(pwd);
            LoginBtn.Click();
        }

        public void LogOutfromApplication()
        {
            Thread.Sleep(5000);
            LogOut.Click();
            Thread.Sleep(3000);
            LogOutOne.Click();
            Thread.Sleep(3000);
        }
    }
}
