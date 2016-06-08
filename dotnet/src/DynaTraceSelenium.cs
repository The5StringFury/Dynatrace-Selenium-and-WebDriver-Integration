using Selenium;
using System;


namespace DynatraceCSharpHelper
{

    /**
     * In order for Timer Names to work correctly we need to override open and openWindow
     * The JavaScript injection of TimerNames only works once the browser starts requesting an URL. 
     * To also assign this first activity to a timername we need to pass the timer Name via the URL to the browser agent
     * @author andreas.grabner
     *
     */
    public class DynaTraceSelenium : DefaultSelenium
    {


        public DynaTraceSeleniumHelper dynaTraceSeleniumHelper;

        public DynaTraceSelenium(String host, int port, String browser, String startUrl) : base(host, port, browser, startUrl)
        {

            dynaTraceSeleniumHelper = new DynaTraceSeleniumHelper(this);
        }

        
        public DynaTraceSelenium(ICommandProcessor commandProcessor):base(commandProcessor)
        {
            dynaTraceSeleniumHelper = new DynaTraceSeleniumHelper(this);
        }

        /**
         * Setting the TimerName, e.g.: from the SeleniumRunner by passing the TestMethod name 
         * @param timerNames
         */
        public void setTimerName(string[] timerNames)
        {
            dynaTraceSeleniumHelper.setTimerName(timerNames);
        }

        /**
         * Add additional timer names to the current timer hierarchy
         * @param addTimerNames
         */
        public void addTimerName(string[] addTimerNames)
        {
            dynaTraceSeleniumHelper.addTimerName(addTimerNames);
        }

        /**
         * Removes the last specified Timer Name hierarchy
         */
        public void removeTimerName()
        {
            dynaTraceSeleniumHelper.removeTimerName();
        }

        /**
         * Clear the timer, e.g.: by the SeleniumRunner after a test method has finished
         */
        public void clearTimerName()
        {
            dynaTraceSeleniumHelper.clearTimerName();
        }

        /**
         * Need to override this method and add the timerName to the URL
         * This is a technical limitation as the JavaScript injection of the timername doesn't work until the browser starts requesting the first url
         */

        public void open(string url)
        {
            dynaTraceSeleniumHelper.setTimerName(true);
            //(dynaTraceSeleniumHelper.getDynaTraceTimerUrl(url));
        }

        /**
         * Need to override this method and add the timerName to the URL
         * This is a technical limitation as the JavaScript injection of the timername doesn't work until the browser starts requesting the first url
         */

        public void openWindow(string url, string windowID)
        {
            dynaTraceSeleniumHelper.setTimerName(true);
            //base(dynaTraceSeleniumHelper.getDynaTraceTimerUrl(url), windowID);
        }
    }
}
