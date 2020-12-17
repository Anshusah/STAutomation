using OpenQA.Selenium;
using System;

namespace SimpleTransferUITest.Helper
{
    public class JavaScriptHelper
    {
        public IWebDriver _driver;

        public JavaScriptHelper(IWebDriver _driver)
        {
            this._driver = _driver;
        }

        public Object ExecuteScript(string script, int x, int y)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            return js.ExecuteScript(script);
        }

        public Object ExecuteScript(String script, Object args)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            return js.ExecuteScript(script, args);
        }


        public void scrollIntoView(IWebElement element)
        {
            ExecuteScript("arguments[0].scrollIntoView()", element);
        }

        public void scrollIntoViewAndClick(IWebElement element)
        {
            scrollIntoView(element);
            element.Click();
        }


        public void scrollToElement(IWebElement element)
        {
            ExecuteScript("window.scrollTo(arguments[0],arguments[1])", element.Location.X, element.Location.Y);
        }

        public void scrollToElemetAndClick(IWebElement element)
        {
            scrollToElement(element);
            element.Click();
        }

    }
}
