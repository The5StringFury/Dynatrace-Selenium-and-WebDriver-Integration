using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DynatraceCSharpHelper
{
    public class DynaTraceWebDriver : IWebDriver
    {
        private IWebDriver driver;
        private DynaTraceWebDriverNavigation dynaTraceNavigation;
        private static Dictionary<IWebDriver, DynaTraceWebDriver> drivers = new Dictionary<IWebDriver, DynaTraceWebDriver>();

        public DynaTraceWebDriver(IWebDriver driver)
        {
            this.driver = driver;
            this.dynaTraceNavigation = new DynaTraceWebDriverNavigation(driver);
        }

        public static DynaTraceWebDriver forWebDriver(IWebDriver driver)
        {
            DynaTraceWebDriver dtDriver = null;

            drivers.TryGetValue(driver, out dtDriver);
            if (dtDriver == null)
            {
                dtDriver = new DynaTraceWebDriver(driver);
                drivers.Add(driver, dtDriver);
            }
            return dtDriver;
        }

        public string CurrentWindowHandle
        {
            get
            {
                return driver.CurrentWindowHandle;
            }
        }

        public string PageSource
        {
            get
            {
                return driver.PageSource;
            }
        }

        public string Title
        {
            get
            {
                return driver.Title;
            }
        }

        public string Url
        {
            get
            {
                return driver.Url;
            }

            set
            {
                driver.Url = Url;
            }
        }

        public ReadOnlyCollection<string> WindowHandles
        {
            get
            {
                return driver.WindowHandles;
            }
        }

        public void Close()
        {
            driver.Close();
        }

        public void Dispose()
        {
            driver.Dispose();
        }

        public IWebElement FindElement(By by)
        {
            return driver.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return driver.FindElements(by);
        }

        public IOptions Manage()
        {
            return driver.Manage();
        }

        public INavigation Navigate()
        {
            return driver.Navigate();
        }

        public void Quit()
        {
            driver.Quit();
        }

        public ITargetLocator SwitchTo()
        {
            return driver.SwitchTo();
        }

        private class DynaTraceWebDriverNavigation : INavigation
        {
            protected IWebDriver driver;
            protected DynaTraceWebDriverHelper dynaTrace;
            public DynaTraceWebDriverNavigation(IWebDriver driver)
            {
                this.driver = driver;
                dynaTrace = DynaTraceWebDriverHelper.forDriver(driver);
            }


            public void Back()
            {
                driver.Navigate().Back();
            }

            public void Forward()
            {
                driver.Navigate().Forward();
            }

            public void GoToUrl(Uri url)
            {                              
                    driver.Navigate().GoToUrl(url);             
            }

            public void GoToUrl(string url)
            {
                try
                {
                    driver.Navigate().GoToUrl(url);
                }
                catch (UriFormatException)
                {   
                }
            }

            public void Refresh()
            {
                driver.Navigate().Refresh();
            }
        }

        public IWebDriver getWebDriver()
        {
            return driver;
        }
    }

   
}