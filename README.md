# Selenium and WebDriver Integration

## Overview

![images_community/download/attachments/16089385/icon.png](images_community/download/attachments/16089385/icon.png)

[Selenium](http://seleniumhq.org/) is a free Web Application Testing System which has gained a lot of popularity since it became available. Its integration into FireFox - which allows instant
record/replay as well as the scripting option - makes it a great tool for functional web testing.  
[BrowserMob](http://browsermob.com/) on the other side leverages Selenium by providing an automated load-testing environment for Selenium scripts all configurable online via a Web Dashboard.  
Selenium also offers writing the test scripts as JUnit tests. With a special dynaTrace JUnit Runner and an extension to the Selenium base functionality it is now possible to tag web requests that are
executed by a Selenium Java Test.

## Plugin Details

| Name |Selenium and WebDriver Integration
| :--- | :---
| Author | Andreas Grabner (andreas.grabner@dynatrace.com)
| Supported dynaTrace Versions | >= 5.5
| Supported Selenium Versions | Tested with Selenium 1.0.1 & 2.0b1
| Support | [Not Supported ](https://community.compuwareapm.com/community/display/DL/Support+Levels#SupportLevels-Community)
| Release history | Version 1.0
| Download | [Selenium Helper Classes](SeleniumHelpers.zip)  
| | [Demo Applications](https://community.compuwareapm.com/community/display/DL/Demo+Applications)

## Installation and First Steps

  * Start by reading **[dynaTrace in Continuous Integration - The Big Picture](https://community.compuwareapm.com/community/display/PUB/dynaTrace+in+Continuous+Integration+-+The+Big+Picture)** which explains how dynaTrace Integrates in your CI Environment. 

  * Learn **[How to include dynaTrace in your Selenium Tests](https://community.compuwareapm.com/community/display/PUB/How+to+include+dynaTrace+in+your+Selenium+Tests)**

  * Download the attached **[Selenium Helper Classes](SeleniumHelpers.zip)** or the full **[Demo Applications](https://community.compuwareapm.com/community/display/DL/Demo+Applications)** Packages 

If you are using Maven and WebDriver - read the following Guide: **[MavenWebDriverIntegration.docx](MavenWebDriverIntegration.docx)**

### Helper Classes

The package contains

  * Helper Classes (com.dynatrace.webautomation.*) for both Selenium and WebDriver. If your Selenium Package doesn't include WebDriver simply delete the WebDriver Helper Classes. 

  * A sample build.xml file that shows how to inject dynaTrace into the JUnit Task that executes the Selenium Tests 

