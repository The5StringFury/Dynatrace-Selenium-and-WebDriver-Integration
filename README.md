<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Selenium and WebDriver Integration</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <meta content="Scroll Wiki Publisher" name="generator"/>
    <link type="text/css" rel="stylesheet" href="css/blueprint/liquid.css" media="screen, projection"/>
    <link type="text/css" rel="stylesheet" href="css/blueprint/print.css" media="print"/>
    <link type="text/css" rel="stylesheet" href="css/content-style.css" media="screen, projection, print"/>
    <link type="text/css" rel="stylesheet" href="css/screen.css" media="screen, projection"/>
    <link type="text/css" rel="stylesheet" href="css/print.css" media="print"/>
</head>
<body>
                <h1>Selenium and WebDriver Integration</h1>
    <div class="section-2"  id="16089385_SeleniumandWebDriverIntegration-Overview"  >
        <h2>Overview</h2>
    <p>
            <img src="images_community/download/attachments/16089385/icon.png" alt="images_community/download/attachments/16089385/icon.png" class="confluence-embedded-image image-center" />
            </p>
    <p>
<a href="http://seleniumhq.org/">Selenium</a> is a free Web Application Testing System which has gained a lot of popularity since it became available. Its integration into FireFox - which allows instant record/replay as well as the scripting option - makes it a great tool for functional web testing.<br/><a href="http://browsermob.com/">BrowserMob</a> on the other side leverages Selenium by providing an automated load-testing environment for Selenium scripts all configurable online via a Web Dashboard.<br/>Selenium also offers writing the test scripts as JUnit tests. With a special dynaTrace JUnit Runner and an extension to the Selenium base functionality it is now possible to tag web requests that are executed by a Selenium Java Test.    </p>
    </div>
    <div class="section-2"  id="16089385_SeleniumandWebDriverIntegration-PluginDetails"  >
        <h2>Plugin Details</h2>
    <div class="tablewrap">
        <table>
<thead class=" "></thead><tfoot class=" "></tfoot><tbody class=" ">    <tr>
            <td rowspan="1" colspan="1">
        <p>
Name    </p>
            </td>
                <td rowspan="1" colspan="1">
        <p>
<strong class=" ">Selenium and WebDriver Integration</strong>    </p>
            </td>
        </tr>
    <tr>
            <td rowspan="1" colspan="1">
        <p>
Version    </p>
            </td>
                <td rowspan="1" colspan="1">
        <p>
1.0    </p>
            </td>
        </tr>
    <tr>
            <td rowspan="1" colspan="1">
        <p>
Author    </p>
            </td>
                <td rowspan="1" colspan="1">
        <p>
Andreas Grabner (andreas.grabner@dynatrace.com)    </p>
            </td>
        </tr>
    <tr>
            <td rowspan="1" colspan="1">
        <p>
Support    </p>
            </td>
                <td rowspan="1" colspan="1">
        <p>
<a href="https://community/display/DL/Support+Levels#SupportLevels-Community">Not Supported </a>    </p>
            </td>
        </tr>
    <tr>
            <td rowspan="1" colspan="1">
        <p>
Download    </p>
            </td>
                <td rowspan="1" colspan="1">
        <p>
<a href="attachments_45514757_1_SeleniumHelpers.zip">Selenium Helper Classes</a><br/><a href="https://community/display/DL/Demo+Applications">Demo Applications</a>    </p>
            </td>
        </tr>
    <tr>
            <td rowspan="1" colspan="1">
        <p>
Tested with    </p>
            </td>
                <td rowspan="1" colspan="1">
        <p>
Selenium 1.0.1 &amp; 2.0b1; dynaTrace v3.5+    </p>
            </td>
        </tr>
</tbody>        </table>
            </div>
    </div>
    
    <div class="section-2"  id="16089385_SeleniumandWebDriverIntegration-InstallationandFirstSteps"  >
        <h2>Installation and First Steps</h2>
    
<ul class=" "><li class=" ">    <p>
Start by reading <strong class=" "><a href="https://community/display/PUB/dynaTrace+in+Continuous+Integration+-+The+Big+Picture">dynaTrace in Continuous Integration - The Big Picture</a></strong> which explains how dynaTrace Integrates in your CI Environment.    </p>
</li><li class=" ">    <p>
Learn <strong class=" "><a href="https://community/display/PUB/How+to+include+dynaTrace+in+your+Selenium+Tests">How to include dynaTrace in your Selenium Tests</a></strong>    </p>
</li><li class=" ">    <p>
Download the attached <strong class=" "><a href="attachments_45514757_1_SeleniumHelpers.zip">Selenium Helper Classes</a></strong> or the full <strong class=" "><a href="https://community/display/DL/Demo+Applications">Demo Applications</a></strong> Packages    </p>
</li></ul>    <p>
If you are using Maven and WebDriver - read the following Guide: <strong class=" "><a href="attachments_93192226_1_MavenWebDriverIntegration.docx">MavenWebDriverIntegration.docx</a></strong>    </p>
    <div class="section-3"  id="16089385_SeleniumandWebDriverIntegration-HelperClasses"  >
        <h3>Helper Classes</h3>
    
    <p>
The package contains    </p>
<ul class=" "><li class=" ">    <p>
Helper Classes (com.dynatrace.webautomation.*) for both Selenium and WebDriver. If your Selenium Package doesn't include WebDriver simply delete the WebDriver Helper Classes.    </p>
</li><li class=" ">    <p>
A sample build.xml file that shows how to inject dynaTrace into the JUnit Task that executes the Selenium Tests    </p>
</li></ul>    </div>
    </div>
            </div>
        </div>
        <div class="footer">
        </div>
    </div>
</body>
</html>