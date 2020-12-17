using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTransferUITest.Helper
{
    public class DropDownHelper
    {
        private IWebDriver _driver;

        public DropDownHelper(IWebDriver driver)
        {
            this._driver = driver;
        }

        public void SelectUsingVisibleValue(IWebElement element, string visibleValue)
        {
            SelectElement oSelect = new SelectElement(element);
            oSelect.SelectByText(visibleValue);
        }


        public void RadioButton(IList<IWebElement> element, string radioValue)
        {
            int Size = element.Count;
            for (int i = 0; i < Size; i++)
            {
                String Value = element.ElementAt(i).GetAttribute("value");

                if (Value.Equals(radioValue))
                {
                    element.ElementAt(i).Click();
                    break;
                }
            }
        }
    }
}
