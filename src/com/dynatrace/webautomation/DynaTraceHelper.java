package com.dynatrace.webautomation;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.text.DateFormat;
import java.util.Date;

public abstract class DynaTraceHelper {

	protected String currentTimerName = null;
	protected boolean enableJavaScriptInjection = true;
	protected boolean dynaTraceAgentActive = true;

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
	public void enableJavaScriptInjection(boolean enable) {
		this.enableJavaScriptInjection = enable;
	}

	/**
	 * Clear the current dynaTrace timer.
	 * All activities in the browser from now on will not be assigned with a timer
	 */
	public void clearTimerName() {
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
	public void setTimerName(String... timerNames) {
		if (timerNames.length > 0) {
			String timerName = null;
			for (String name : timerNames) {
				if (timerName == null)
					timerName = name;
				else
					timerName += "/" + name;
			}
			currentTimerName = TestRunIdProvider.getTestRunId() + "#" + timerName;
		}
		else {
			currentTimerName = null;
		}
		setCurrentTimerNameViaJavaScript();
	}

	/***
	 * Adds additional timer names to the current timer
	 * @param addTimerNames
	 */
	public void addTimerName(String... addTimerNames) {
		if(currentTimerName == null || addTimerNames.length == 0)
			setTimerName(addTimerNames);
		else {
			for (String name : addTimerNames) {
				currentTimerName += "/" + name;
			}
			setCurrentTimerNameViaJavaScript();
		}
	}

	/***
	 * Remove multiple levels of timer names
	 * @param levels
	 */
	public void removeTimerName(int levels) {
		if(currentTimerName == null) return;
		while(levels > 0) {
			int lastDivider = currentTimerName.lastIndexOf("/");
			if(lastDivider < 0) {
				currentTimerName = null;
				return;
			}

			currentTimerName = currentTimerName.substring(0, lastDivider);
			levels--;
		}
		setCurrentTimerNameViaJavaScript();
	}

	/***
	 * @return True if a timername is set
	 */
	public boolean hasTimerName() {
		return (currentTimerName != null) && (currentTimerName.length() > 0);
	}

	/***
	 * Removes the last level of timer names
	 */
	public void removeTimerName() {
		removeTimerName(1);
	}

	/***
	 * Uses the current stack trace and sets the name of the calling method as Timer Name
	 */
	public void setTimerName() {
		setTimerName(false);
	}

	/***
	 * Uses the current stack trace and sets the name of the calling method as Timer Name
	 * If excludeCaller = true the direct caller of setTimerName will be ignored
	 */
	public void setTimerName(boolean excludeCaller) {
		// we walk through the stack trace and pick the first method from outside this class
		StackTraceElement[] stackTrace = new Exception().getStackTrace();
		String helperClassName = this.getClass().getName();
		for(int i = excludeCaller ? 2 : 1; i < stackTrace.length; i++) {
			StackTraceElement elem = stackTrace[i];
			if(elem.getClassName().equals(helperClassName)) continue;
			setTimerName(elem.getClassName(), elem.getMethodName());
			break;
		}
	}


	/***
	 * This helper method returns a modified URL that contains the current timer name as part of the URL
	 * Use this method when opening a new URL that should be tagged and in case setting the timer via JavaScript doesn't work
	 * @param url Input url, e.g.: http://www.google.com
	 * @return this would for instance return http://www.google.com?dtTimerName=myGoogleTimer
	 */
	public String getDynaTraceTimerUrl(String url) {
		if (currentTimerName == null)
			return url;
		try {
			String escapedTimerName = URLEncoder.encode(currentTimerName, "UTF-8").replace("+", "%20");
			if (url.contains("?"))
				return url + "&dtTimerName=" + escapedTimerName;
			return url + "?dtTimerName=" + escapedTimerName;
		} catch (UnsupportedEncodingException e) {
			return url;
		}
	}

	/***
	 * This helper method returns a modified URL with the passed Timer Name as part of the URL
	 * Use this method when opening a new URL that should be tagged and in case setting the timer via JavaScript doesn't work
	 * @param url Input url, e.g.: http://www.google.com
	 * @return this would for instance return http://www.google.com?dtTimerName=myGoogleTimer
	 */
	public String getDynaTraceTimerUrl(String url, String ... timerNames) {
		setTimerName(timerNames);

		if (currentTimerName == null)
			return url;
		try {
			String escapedTimerName = URLEncoder.encode(currentTimerName, "UTF-8").replace("+", "%20");
			if (url.contains("?"))
				return url + "&dtTimerName=" + escapedTimerName;
			return url + "?dtTimerName=" + escapedTimerName;
		} catch (UnsupportedEncodingException e) {
			return url;
		}
	}

	/**
	 * Opens a new URL and sets a new timer name with the calling method
	 * @param url
	 */
	public void open(String url) {
		enableJavaScriptInjection(false);
		setTimerName(true);
		enableJavaScriptInjection(true);

		if(dynaTraceAgentActive)
			openUrl(getDynaTraceTimerUrl(url));
		else
			openUrl(url);
	}

	/**
	 * Opens a new URL
	 * Either uses the existing timername or creates a new one with the callers method name
	 * @param url
	 */
	public void open(String url, boolean useExistingTimerName) {
		if(currentTimerName == null || !useExistingTimerName) {
			// when we browse to a new page we do not need to set the time via JavaScript - we do it via the URL
			enableJavaScriptInjection(false);
			setTimerName(true);
			enableJavaScriptInjection(true);
		}

		if(dynaTraceAgentActive)
			openUrl(getDynaTraceTimerUrl(url));
		else
			openUrl(url);
	}


	/**
	 * Opens a new URL using the passed Timer Name
	 * @param url
	 */
	public void open(String url, String ... timerNames) {
		if(dynaTraceAgentActive)
			openUrl(getDynaTraceTimerUrl(url, timerNames));
		else
			openUrl(url);
	}

	private static class TestRunIdProvider {
		private static String testrunId;

		protected static String getTestRunId() {
			if (testrunId == null) {
				testrunId = System.getProperty("testRunId");
				if (testrunId == null) {
					testrunId = "Run started at " + DateFormat.getDateTimeInstance().format(new Date());
				}
			}
			return testrunId;
		}
	}

}
