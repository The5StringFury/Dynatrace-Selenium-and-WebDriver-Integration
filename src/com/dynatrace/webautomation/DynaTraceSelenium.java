package com.dynatrace.webautomation;


import com.thoughtworks.selenium.CommandProcessor;
import com.thoughtworks.selenium.DefaultSelenium;


/**
 * In order for Timer Names to work correctly we need to override open and openWindow
 * The JavaScript injection of TimerNames only works once the browser starts requesting an URL. 
 * To also assign this first activity to a timername we need to pass the timer Name via the URL to the browser agent
 * @author andreas.grabner
 *
 */
public class DynaTraceSelenium extends DefaultSelenium {	

	private DynaTraceSeleniumHelper dynaTraceSeleniumHelper;
	
	public DynaTraceSelenium(String host, int port, String browser, String startUrl) {
        super(host, port, browser, startUrl);
        
        dynaTraceSeleniumHelper = new DynaTraceSeleniumHelper(this);
    }

    public DynaTraceSelenium(CommandProcessor commandProcessor) {
        super(commandProcessor);
        
        dynaTraceSeleniumHelper = new DynaTraceSeleniumHelper(this);
    }
        
    /**
     * Setting the TimerName, e.g.: from the SeleniumRunner by passing the TestMethod name 
     * @param timerNames
     */
    public void setTimerName(String ... timerNames) {
    	dynaTraceSeleniumHelper.setTimerName(timerNames);
    }
    
    /**
     * Add additional timer names to the current timer hierarchy
     * @param addTimerNames
     */
    public void addTimerName(String... addTimerNames) {
    	dynaTraceSeleniumHelper.addTimerName(addTimerNames);
    }
    
    /**
     * Removes the last specified Timer Name hierarchy
     */
    public void removeTimerName() {
    	dynaTraceSeleniumHelper.removeTimerName();
    }
    
    /**
     * Clear the timer, e.g.: by the SeleniumRunner after a test method has finished
     */
    public void clearTimerName() {
    	dynaTraceSeleniumHelper.clearTimerName();
    }

    /**
     * Need to override this method and add the timerName to the URL
     * This is a technical limitation as the JavaScript injection of the timername doesn't work until the browser starts requesting the first url
     */
    @Override
	public void open(String url) {   	
    	dynaTraceSeleniumHelper.setTimerName(true);
		super.open(dynaTraceSeleniumHelper.getDynaTraceTimerUrl(url));
	}

    /**
     * Need to override this method and add the timerName to the URL
     * This is a technical limitation as the JavaScript injection of the timername doesn't work until the browser starts requesting the first url
     */
	@Override
	public void openWindow(String url, String windowID) {
    	dynaTraceSeleniumHelper.setTimerName(true);
		super.openWindow(dynaTraceSeleniumHelper.getDynaTraceTimerUrl(url), windowID);
	}

}
