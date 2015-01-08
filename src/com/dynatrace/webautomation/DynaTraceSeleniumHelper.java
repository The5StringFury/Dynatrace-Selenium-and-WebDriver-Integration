package com.dynatrace.webautomation;

import java.util.HashMap;

import com.thoughtworks.selenium.Selenium;

public class DynaTraceSeleniumHelper extends DynaTraceHelper {

	private Selenium defaultSelenium;

	// allow our javascript injection to fail twice. We might not be active when the browser starts before we actually browse to a URL
	private int failedJSAttempts = 2;

	protected DynaTraceSeleniumHelper(Selenium defaultSelenium) {
		this.defaultSelenium = defaultSelenium;
	}

	private static HashMap<Selenium, DynaTraceSeleniumHelper> driverMap = new HashMap<Selenium, DynaTraceSeleniumHelper>();
	public static DynaTraceSeleniumHelper forSelenium(Selenium defaultSelenium) {
		DynaTraceSeleniumHelper helper = driverMap.get(defaultSelenium);
		if(helper == null) {
			helper = new DynaTraceSeleniumHelper(defaultSelenium);
			driverMap.put(defaultSelenium, helper);
		}
		return helper;
	}

	/***
	 * Sets a dynaTrace Marker on the current timestamp
	 * @param marker
	 */
	@Override
	public void addMark(String marker) {
		try {
			if (marker != null)
				defaultSelenium.runScript("try { _dt_addMark('" + marker + "') } catch(e) { }");
		}
		catch (com.thoughtworks.selenium.SeleniumException e) {
			// The exception "Current window or frame is closed!" can happen.
			// If this exception occurs, the js-call might have happened too soon (before selenium.open() was called).
			// In these cases the timer will get set as soon as selenium.open() gets called, as we override this
			// method and send the timer name with each url.
			if(--failedJSAttempts == 0) {
				dynaTraceAgentActive = false;
				System.out.println("dynaTrace Browser Agent not active!");
			}
		}
	}

	@Override
	protected void setCurrentTimerNameViaJavaScript() {
		if(!enableJavaScriptInjection) return;

		try {
			if (currentTimerName != null)
				defaultSelenium.runScript("try { _dt_setTimerName('" + currentTimerName + "') } catch(e) { }");
			else
				defaultSelenium.runScript("try { _dt_setTimerName() } catch(e) { }");
		}
		catch (com.thoughtworks.selenium.SeleniumException e) {
			// The exception "Current window or frame is closed!" can happen.
			// If this exception occurs, the js-call might have happened too soon (before selenium.open() was called).
			// In these cases the timer will get set as soon as selenium.open() gets called, as we override this
			// method and send the timer name with each url.
			if(--failedJSAttempts == 0) {
				dynaTraceAgentActive = false;
				System.out.println("dynaTrace Browser Agent not active!");
			}
		}
	}

	@Override
	protected void openUrl(String url) {
		defaultSelenium.open(url);
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
