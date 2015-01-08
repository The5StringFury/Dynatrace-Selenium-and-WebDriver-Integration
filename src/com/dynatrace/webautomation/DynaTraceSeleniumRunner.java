package com.dynatrace.webautomation;


import java.lang.reflect.Field;
import java.util.List;

import org.junit.internal.runners.statements.InvokeMethod;
import org.junit.runners.BlockJUnit4ClassRunner;
import org.junit.runners.model.FrameworkMethod;
import org.junit.runners.model.InitializationError;
import org.junit.runners.model.Statement;


public class DynaTraceSeleniumRunner extends BlockJUnit4ClassRunner {

	Field seleniumClientField;
	SeleniumConfiguration seleniumClientAnnotation;

    public DynaTraceSeleniumRunner(Class<?> testcaseClass) throws InitializationError {
    	super(testcaseClass);
    }

    @Override
    protected void collectInitializationErrors(List<Throwable> errors) {
    	for(Field field : getTestClass().getJavaClass().getDeclaredFields()) {
    		SeleniumConfiguration annotation = field.getAnnotation(SeleniumConfiguration.class);
    		if (annotation != null) {
    			seleniumClientField = field;
    			seleniumClientAnnotation = annotation;
    			break;
    		}
    	}
    	if (seleniumClientField == null || !seleniumClientField.getType().isAssignableFrom(DynaTraceSelenium.class)) {
            errors.add(new Exception("Test must provide a field of type com.thoughtworks.selenium.Selenium with the SeleniumConfiguration annotation."));
        }
    	super.collectInitializationErrors(errors);
    }

    @Override
	protected Statement methodInvoker(FrameworkMethod method, Object test) {

        final String methodName = method.getName();
        final String testClassName = test.getClass().getCanonicalName();
        final Object testClassInstance = test;

        return new InvokeMethod(method, test) {
            @Override
            public void evaluate() throws Throwable {
            	// We use the dynaTrace Selenium class because we need to make sure to pass the dynaTrace Timer in the "open" and "openWindow" method
            	DynaTraceSelenium dynaTraceSelenium = new DynaTraceSelenium(seleniumClientAnnotation.seleniumServer(), seleniumClientAnnotation.seleniumPort(), getBrowser(seleniumClientAnnotation), seleniumClientAnnotation.location());
                try {
                	dynaTraceSelenium.start();
                    seleniumClientField.setAccessible(true);
                    seleniumClientField.set(testClassInstance, dynaTraceSelenium);
                    dynaTraceSelenium.setTimerName(testClassName, methodName);
                    super.evaluate();
                } finally {
                	dynaTraceSelenium.clearTimerName();
                	dynaTraceSelenium.stop();
                }
            }

			private String getBrowser(SeleniumConfiguration seleniumClientAnnotation) {
				if (seleniumClientAnnotation.browser().length() > 0)
					return seleniumClientAnnotation.browser();
				return System.getProperty("seleniumBrowser", "*iexplore");
			}
        };
    }

}
