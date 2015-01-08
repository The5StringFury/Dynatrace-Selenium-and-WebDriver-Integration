package com.dynatrace.webautomation;

import java.util.HashMap;

import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebDriverException;

public class DynaTraceWebDriverHelper extends DynaTraceHelper {

	private WebDriver defaultDriver;
	private JavascriptExecutor jsExecutor;

	// allow our javascript injection to fail twice. We might not be active when the browser starts before we actually browse to a URL
	private int failedJSAttempts = 2;

	protected DynaTraceWebDriverHelper(WebDriver defaultDriver) {
		this.defaultDriver = defaultDriver;

		if(defaultDriver instanceof DynaTraceWebDriver)
			this.jsExecutor = (JavascriptExecutor)((DynaTraceWebDriver)defaultDriver).getWebDriver();
		else
			this.jsExecutor = (JavascriptExecutor)defaultDriver;
	}

	private static HashMap<WebDriver, DynaTraceWebDriverHelper> driverMap = new HashMap<WebDriver, DynaTraceWebDriverHelper>();
	public static DynaTraceWebDriverHelper forDriver(WebDriver defaultDriver) {
		if(defaultDriver instanceof DynaTraceWebDriver)
			defaultDriver = ((DynaTraceWebDriver)defaultDriver).getWebDriver();

		DynaTraceWebDriverHelper helper = driverMap.get(defaultDriver);
		if(helper == null) {
			helper = new DynaTraceWebDriverHelper(defaultDriver);
			driverMap.put(defaultDriver, helper);
		}
		return helper;
	}

	@Override
	protected void setCurrentTimerNameViaJavaScript() {
		if(!enableJavaScriptInjection) return;
		if(!dynaTraceAgentActive) return;

		try {
			if (currentTimerName != null)
				// jsExecutor.executeScript("try { _dt_setTimerName('" + currentTimerName + "') } catch(e) { }");
				jsExecutor.executeScript("_dt_setTimerName('" + currentTimerName + "')");
			else
				// jsExecutor.executeScript("try { _dt_setTimerName() } catch(e) { }");
				jsExecutor.executeScript("_dt_setTimerName()");
		}
		catch (WebDriverException e) {
			if(--failedJSAttempts == 0) {
				dynaTraceAgentActive = false;
				System.out.println("dynaTrace Browser Agent not active!");
			}
		}
	}

	@Override
	public void addMark(String marker) {
		try {
			if (marker != null && dynaTraceAgentActive)
				// jsExecutor.executeScript("try { _dt_addMark('" + marker + "') } catch(e) { }");
				jsExecutor.executeScript("_dt_addMark('" + marker + "')");
		}
		catch (WebDriverException e) {
			if(--failedJSAttempts == 0) {
				dynaTraceAgentActive = false;
				System.out.println("dynaTrace Browser Agent not active!");
			}
		}
	}

	@Override
	protected void openUrl(String url) {
		defaultDriver.navigate().to(url);
	}
}
