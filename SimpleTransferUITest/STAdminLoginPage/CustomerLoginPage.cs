using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using System.Threading;

namespace SimpleTransferUITest.STAdminLoginPage
{
    class CustomerLoginPage
    {
        private IWebDriver _driver;

        public CustomerLoginPage(IWebDriver driver)
        {
            _driver = driver;
        }
        public IWebElement LoginBtn => _driver.FindElement(By.XPath("//a[contains(text(),'Login')]"));
        public IWebElement TextUserName => _driver.FindElement(By.XPath("//input[@id='Email']"));
        public IWebElement TextPassword => _driver.FindElement(By.XPath("//input[@id='Password']"));
        public IWebElement Login => _driver.FindElement(By.XPath("//button[@class='btn btn-block btn-primary']"));


        public IWebElement Profile => _driver.FindElement(By.XPath("//span[@class='ml-2 ']"));
        public IWebElement LogOut => _driver.FindElement(By.XPath("//a[contains(text(),'Logout')]"));

        public void ValidLogin(string uName, string pwd)
        {
            LoginBtn.Click();
            TextUserName.SendKeys(uName);
            TextPassword.SendKeys(pwd);
            Login.Click();
        }

        public void LogOutfromApplication()
        {
            Thread.Sleep(5000);
            Profile.Click();
            Thread.Sleep(3000);
            LogOut.Click();
            Thread.Sleep(3000);
        }
    }
}
