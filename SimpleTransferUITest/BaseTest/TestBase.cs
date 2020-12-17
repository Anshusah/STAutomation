using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SimpleTransferUITest.BaseTest
{
    public class TestBase
    {
        public static IWebDriver _driver;
        public void LaunchBrowseradmin()
        {

            //string s1 = ConfigurationManager.AppSettings["url"];
            //Console.WriteLine(s1);

            // ChromeOptions option = new ChromeOptions();
            //option.AddArgument("--headless");
            //string adminurl = "https://simpletransferwebservice-test.azurewebsites.net/st/adminuser/login.html";
            //string customerurl = "https://simpletransferwebservice-test.azurewebsites.net/admin/form/transfer/transfer/ADA=/edit.html";
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            // _driver.Navigate().GoToUrl("https://simpletransferwebservice-test.azurewebsites.net/st/adminuser/login.html");
            _driver.Url = "https://simpletransferwebservice-test.azurewebsites.net/st/adminuser/login.html";
        }
       /* public void LaunchBrowsercustomer()
        {

  
            //string adminurl = "https://simpletransferwebservice-test.azurewebsites.net/st/adminuser/login.html";
            //string customerurl = "https://simpletransferwebservice-test.azurewebsites.net/admin/form/transfer/transfer/ADA=/edit.html";
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl("https://simpletransferwebservice-test.azurewebsites.net/admin/form/transfer/transfer/ADA=/edit.html");
        }*/

        public void CloseBrowser()
        {
            _driver.Quit();
        }
    }
}
