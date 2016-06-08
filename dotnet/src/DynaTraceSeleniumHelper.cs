using System;
using System.Collections.Generic;
using Selenium;


namespace DynatraceCSharpHelper
{
    public class DynaTraceSeleniumHelper : DynaTraceHelper
    {


        private ISelenium defaultSelenium;

        // allow our javascript injection to fail twice. We might not be active when the browser starts before we actually browse to a URL
        private int failedJSAttempts = 2;

        public DynaTraceSeleniumHelper(ISelenium defaultSelenium)
        {
            this.defaultSelenium = defaultSelenium;
        }

        private static Dictionary<ISelenium, DynaTraceSeleniumHelper> driverMap = new Dictionary<ISelenium, DynaTraceSeleniumHelper>();
        public static DynaTraceSeleniumHelper forSelenium(ISelenium defaultSelenium)
        {
            DynaTraceSeleniumHelper helper;

            driverMap.TryGetValue(defaultSelenium,out helper);

            if (helper == null)
            {
                helper = new DynaTraceSeleniumHelper(defaultSelenium);
                driverMap.Add(defaultSelenium, helper);
            }
            return helper;
        }

        /***
         * Sets a dynaTrace Marker on the current timestamp
         * @param marker
         */
      
    public override void addMark(String marker)
        {
            try
            {
                if (marker != null)
                    defaultSelenium.RunScript("try { _dt_addMark('" + marker + "') } catch(e) { }");
            }
            catch (Selenium.SeleniumException)
            {
                // The exception "Current window or frame is closed!" can happen.
                // If this exception occurs, the js-call might have happened too soon (before selenium.open() was called).
                // In these cases the timer will get set as soon as selenium.open() gets called, as we override this
                // method and send the timer name with each url.
                if (--failedJSAttempts == 0)
                {
                    dynaTraceAgentActive = false;
                    Console.WriteLine("dynaTrace Browser Agent not active!");
                }
            }
        }

     
    protected override void setCurrentTimerNameViaJavaScript()
        {
            if (!enableJavaScriptInjection) return;

            try
            {
                if (currentTimerName != null)
                    defaultSelenium.RunScript("try { _dt_setTimerName('" + currentTimerName + "') } catch(e) { }");
                else
                    defaultSelenium.RunScript("try { _dt_setTimerName() } catch(e) { }");
            }
            catch (Selenium.SeleniumException)
            {
                // The exception "Current window or frame is closed!" can happen.
                // If this exception occurs, the js-call might have happened too soon (before selenium.open() was called).
                // In these cases the timer will get set as soon as selenium.open() gets called, as we override this
                // method and send the timer name with each url.
                if (--failedJSAttempts == 0)
                {
                    dynaTraceAgentActive = false;
                    Console.WriteLine("dynaTrace Browser Agent not active!");
                }
            }
        }


    protected override void openUrl(string url)
        {
            defaultSelenium.Open(url);
        }



        /*
         *
            public void open(String url) {
                if(currentTimerName == null) {
                    // when we browse to a new page we do not need to set the time via JavaScript - we do it via the URL
                    enableJavaScriptInjection(false);
                    setTimerName();
                    enableJavaScriptInjection(true);
                }
                defaultSelenium.open(getDynaTraceTimerUrl(url));
            }
            public void open(String url, String ... timerNames) {
                defaultSelenium.open(getDynaTraceTimerUrl(url, timerNames));
            }
            public void openWindow(String url, String windowID) {
                if(currentTimerName == null) {
                    // when we browse to a new page we do not need to set the time via JavaScript - we do it via the URL
                    enableJavaScriptInjection(false);
                    setTimerName();
                    enableJavaScriptInjection(true);
                }
                defaultSelenium.openWindow(getDynaTraceTimerUrl(url), windowID);
            }
            public void openWindow(String url, String windowID, String ... timerNames) {
                defaultSelenium.openWindow(getDynaTraceTimerUrl(url, timerNames), windowID);
            }
         */
    }
}


