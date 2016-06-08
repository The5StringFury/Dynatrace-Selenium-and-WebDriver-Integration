using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace DynatraceCSharpHelper
{
    public class DynaTraceWebDriverHelper : DynaTraceHelper
    {


        private IWebDriver defaultDriver;
        private IJavaScriptExecutor jsExecutor;

        // allow our javascript injection to fail twice. We might not be active when the browser starts before we actually browse to a URL
        private int failedJSAttempts = 2;

        protected DynaTraceWebDriverHelper(IWebDriver defaultDriver)
        {
            this.defaultDriver = defaultDriver;

            if (defaultDriver.GetType().Equals(typeof(DynaTraceWebDriver)))
            {
                this.jsExecutor = (IJavaScriptExecutor)((DynaTraceWebDriver)defaultDriver).getWebDriver();
            }
            else
            {
                this.jsExecutor = (IJavaScriptExecutor)defaultDriver;
            }
        }

        private static Dictionary<IWebDriver, DynaTraceWebDriverHelper> driverMap = new Dictionary<IWebDriver, DynaTraceWebDriverHelper>();
        public static DynaTraceWebDriverHelper forDriver(IWebDriver defaultDriver)
        {
            if (defaultDriver.GetType().Equals(typeof(DynaTraceWebDriver)))
            {
                defaultDriver = ((DynaTraceWebDriver)defaultDriver).getWebDriver();
            }

            DynaTraceWebDriverHelper helper = null;
            driverMap.TryGetValue(defaultDriver, out helper);
            if (helper == null)
            {
                helper = new DynaTraceWebDriverHelper(defaultDriver);
                driverMap.Add(defaultDriver, helper);
            }
            return helper;
        }


        protected override void setCurrentTimerNameViaJavaScript()
        {
            if (!enableJavaScriptInjection) return;
            if (!dynaTraceAgentActive) return;

            try
            {
                if (currentTimerName != null)
                    // jsExecutor.executeScript("try { _dt_setTimerName('" + currentTimerName + "') } catch(e) { }");
                    jsExecutor.ExecuteScript("_dt_setTimerName('" + currentTimerName + "')");
                else
                    // jsExecutor.executeScript("try { _dt_setTimerName() } catch(e) { }");
                    jsExecutor.ExecuteScript("_dt_setTimerName()");
            }
            catch (WebDriverException)
            {
                if (--failedJSAttempts == 0)
                {
                    dynaTraceAgentActive = false;
                    Console.WriteLine("dynaTrace Browser Agent not active!");
                }
            }
        }


        public override void addMark(String marker)
        {
            try
            {
                if (marker != null && dynaTraceAgentActive)
                    // jsExecutor.executeScript("try { _dt_addMark('" + marker + "') } catch(e) { }");
                    jsExecutor.ExecuteScript("_dt_addMark('" + marker + "')");
            }
            catch (WebDriverException)
            {
                if (--failedJSAttempts == 0)
                {
                    dynaTraceAgentActive = false;
                    Console.WriteLine("dynaTrace Browser Agent not active!");
                }
            }
        }


        protected override void openUrl(String url)
        {
            defaultDriver.Navigate().GoToUrl(url);
        }

    }
}
