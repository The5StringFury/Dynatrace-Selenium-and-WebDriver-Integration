using System;
using System.Diagnostics;


namespace DynatraceCSharpHelper
{
    public abstract class DynaTraceHelper
    {
        protected String currentTimerName = null;
        protected Boolean enableJavaScriptInjection = true;
        protected bool dynaTraceAgentActive = true;

        protected abstract void setCurrentTimerNameViaJavaScript();
        protected abstract void openUrl(String url);

        /***
         * Sets a dynaTrace Marker on the current timestamp
         * @param marker
         */
        public abstract void addMark(String marker);


        /**
         * Allows disabling JavaScript injection of timernames
         * This might be useful in cases before a first page request was executed to avoid an exception in Selenium
         * @param enable
         */
        public void SetenableJavaScriptInjection(bool enable)
        {
            this.enableJavaScriptInjection = enable;
        }

        /**
         * Clear the current dynaTrace timer.
         * All activities in the browser from now on will not be assigned with a timer
         */
        public void clearTimerName()
        {
            currentTimerName = null;
            setCurrentTimerNameViaJavaScript();
        }

        /***
         * Sets a new dynaTrace Timer Name.
         * All activities in the browser from now on will be assigned to that timer
         * Timers in dynaTrace allow you to subscribe measures such as #of Network Requests, Exec time of JavaScript, #of Database Calls, ...
         * -> these measures will automatically be analyzed and regression across builds will be identified for you
         * @param timerNames A list of names will be concatenated with / which allows you to group timers. Example: MyWebSite/Login, MyWebSite/Search, ...
         */
        public void setTimerName(string[] timerNames)
        {
            if (timerNames.Length > 0)
            {
                string timerName = null;
                foreach (string name in timerNames)
                {
                    if (timerName == null)
                        timerName = name;
                    else
                        timerName += "/" + name;
                }
                currentTimerName = TestRunIdProvider.getTestRunId() + "#" + timerName;
            }
            else
            {
                currentTimerName = null;
            }
            setCurrentTimerNameViaJavaScript();
        }

        public void setTimerName(string timerName)
        {
            if (string.IsNullOrEmpty(timerName))
            {
                currentTimerName = null;
            }
            else
            {
                currentTimerName = TestRunIdProvider.getTestRunId() + "#" + timerName;
            }
            
        }

        /***
         * Adds additional timer names to the current timer
         * @param addTimerNames
         */
        public void addTimerName(string[] addTimerNames)
        {
            if (currentTimerName == null || addTimerNames.Length == 0)
                setTimerName(addTimerNames);
            else
            {
                foreach (string name in addTimerNames)
                {
                    currentTimerName += "/" + name;
                }
                setCurrentTimerNameViaJavaScript();
            }
        }

        /***
         * Remove multiple levels of timer names
         * @param levels
         */
        public void removeTimerName(int levels)
        {
            if (currentTimerName == null) return;
            while (levels > 0)
            {
                int lastDivider = currentTimerName.LastIndexOf('/');
                if (lastDivider < 0)
                {
                    currentTimerName = null;
                    return;
                }

                currentTimerName = currentTimerName.Substring(0, lastDivider);
                levels--;
            }
            setCurrentTimerNameViaJavaScript();
        }

        /***
         * @return True if a timername is set
         */
        public bool hasTimerName()
        {
            return (currentTimerName != null) && (currentTimerName != string.Empty);
        }

        /***
         * Removes the last level of timer names
         */
        public void removeTimerName()
        {
            removeTimerName(1);
        }

        /***
         * Uses the current stack trace and sets the name of the calling method as Timer Name
         */
        public void setTimerName()
        {
            setTimerName(false);
        }

        /***
         * Uses the current stack trace and sets the name of the calling method as Timer Name
         * If excludeCaller = true the direct caller of setTimerName will be ignored
         */
        public void setTimerName(bool excludeCaller)
        {
            // we walk through the stack trace and pick the first method from outside this class
            StackTrace stackTrace = new StackTrace();
            StackFrame[] StackTraceFrames = stackTrace.GetFrames();
            string helperClassName = this.GetType().Name;
            for (int i = excludeCaller ? 2 : 1; i < StackTraceFrames.Length; i++)
            {
                StackFrame elem = StackTraceFrames[i];
                if (elem.GetType().Name.Equals(helperClassName))
                {
                    continue;
                }
                else
                {
                    setTimerName((elem.GetType().Name + "," + elem.GetMethod().Name).Split(','));
                    
                }
                break;
            }
        }


        /***
         * This helper method returns a modified URL that contains the current timer name as part of the URL
         * Use this method when opening a new URL that should be tagged and in case setting the timer via JavaScript doesn't work
         * @param url Input url, e.g.: http://www.google.com
         * @return this would for instance return http://www.google.com?dtTimerName=myGoogleTimer
         */
        public String getDynaTraceTimerUrl(String url)
        {
            if (currentTimerName == null)
                return url;
            try
            {
                string escapedTimerName = System.Web.HttpUtility.UrlEncode(currentTimerName,System.Text.ASCIIEncoding.UTF8).Replace("+", "%20");
                if (url.Contains("?"))
                    return url + "&dtTimerName=" + escapedTimerName;
                return url + "?dtTimerName=" + escapedTimerName;
            }
            catch (Exception)
            {
                return url;
            }
        }

        /***
         * This helper method returns a modified URL with the passed Timer Name as part of the URL
         * Use this method when opening a new URL that should be tagged and in case setting the timer via JavaScript doesn't work
         * @param url Input url, e.g.: http://www.google.com
         * @return this would for instance return http://www.google.com?dtTimerName=myGoogleTimer
         */
        public String getDynaTraceTimerUrl(String url, string timerNames)
        {
            setTimerName(timerNames);

            if (currentTimerName == null)
                return url;
            try
            {
                string escapedTimerName = System.Web.HttpUtility.UrlEncode(currentTimerName, System.Text.ASCIIEncoding.UTF8).Replace("+", "%20");
                if (url.Contains("?"))
                    return url + "&dtTimerName=" + escapedTimerName;
                return url + "?dtTimerName=" + escapedTimerName;
            }
            catch (Exception)
            {
                return url;
            }
        }

        /**
         * Opens a new URL and sets a new timer name with the calling method
         * @param url
         */
        public void open(String url)
        {
            setEnableJavaScriptInjection(false);
            setTimerName(true);
            setEnableJavaScriptInjection(true);

            if (dynaTraceAgentActive)
                openUrl(getDynaTraceTimerUrl(url));
            else
                openUrl(url);
        }

        private void setEnableJavaScriptInjection(bool v)
        {
            enableJavaScriptInjection = v;
        }

        /**
         * Opens a new URL
         * Either uses the existing timername or creates a new one with the callers method name
         * @param url
         */
        public void open(String url, bool useExistingTimerName)
        {
            if (currentTimerName == null || !useExistingTimerName)
            {
                // when we browse to a new page we do not need to set the time via JavaScript - we do it via the URL
                setEnableJavaScriptInjection(false);
                setTimerName(true);
                setEnableJavaScriptInjection(true);
            }

            if (dynaTraceAgentActive)
                openUrl(getDynaTraceTimerUrl(url));
            else
                openUrl(url);
        }


        /**
         * Opens a new URL using the passed Timer Name
         * @param url
         */
        public void open(String url, string timerNames)
        {
            if (dynaTraceAgentActive)
                openUrl(getDynaTraceTimerUrl(url, timerNames));
            else
                openUrl(url);
        }

        public static class TestRunIdProvider
        {
            private static string testrunId;

            public static string getTestRunId()
            {
                if (testrunId == null)
                {
                    StackTrace stackTrace = new StackTrace();
                    StackFrame[] StackTraceFrames = stackTrace.GetFrames();
                    foreach (StackFrame X in StackTraceFrames)
                    {
                        try
                        {
                            testrunId = X.GetType().GetProperty("testRunId").Name;
                            break;
                        }
                        catch (NullReferenceException)
                        {
                            continue;
                        }
                    }
                    if (testrunId == null)
                    {
                        testrunId = "Run started at " + DateTime.Now;
                    }

                }
                return testrunId;
            }
        }
    }
}
