using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Stavworld_Csharp_Selenium_Specflow_Nunit.Utility
{
    /// WaitHelper class provides common wait methods for Selenium WebDriver
    public class WaitHelper
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private const int DEFAULT_TIMEOUT = 15; // Default timeout in seconds

        public WaitHelper(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        /// Wait for page to load completely
        /// <param name="timeoutInSeconds">Timeout in seconds (default: DEFAULT_TIMEOUT)</param>
        /// <returns>True if page loaded successfully</returns>
        public bool WaitForPageToLoad(int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                
                // Wait for document ready state
                wait.Until(driver => 
                {
                    var jsExecutor = (IJavaScriptExecutor)driver;
                    return jsExecutor.ExecuteScript("return document.readyState").Equals("complete");
                });

                // Wait for jQuery to be loaded and ready (if present)
                try
                {
                    wait.Until(driver => 
                    {
                        var jsExecutor = (IJavaScriptExecutor)driver;
                        return (bool)jsExecutor.ExecuteScript("return typeof jQuery === 'undefined' || jQuery.active === 0");
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    // jQuery not present or not ready, continue
                }

                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Page load timeout after {timeoutInSeconds} seconds");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error waiting for page load: {ex.Message}");
                return false;
            }
        }

        /// Wait for page to load with default timeout
        /// <returns>True if page loaded successfully</returns>
        public bool WaitForPageToLoad()
        {
            return WaitForPageToLoad(DEFAULT_TIMEOUT);
        }

        /// Wait for element to be visible
        /// <param name="locator">Element locator</param>
        /// <param name="timeoutInSeconds">Timeout in seconds (default: DEFAULT_TIMEOUT)</param>
        /// <returns>True if element is visible</returns>
        public bool WaitForElementVisible(By locator, int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(locator));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Element not visible after {timeoutInSeconds} seconds: {locator}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error waiting for element visibility: {ex.Message}");
                return false;
            }
        }

        /// Wait for element to be visible with default timeout
        /// <param name="locator">Element locator</param>
        /// <returns>True if element is visible</returns>
        public bool WaitForElementVisible(By locator)
        {
            return WaitForElementVisible(locator, DEFAULT_TIMEOUT);
        }

        /// Wait for element to be clickable
        /// <param name="locator">Element locator</param>
        /// <param name="timeoutInSeconds">Timeout in seconds (default: DEFAULT_TIMEOUT)</param>
        /// <returns>True if element is clickable</returns>
        public bool WaitForElementClickable(By locator, int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Element not clickable after {timeoutInSeconds} seconds: {locator}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error waiting for element clickability: {ex.Message}");
                return false;
            }
        }

        /// Wait for element to be clickable with default timeout
        /// <param name="locator">Element locator</param>
        /// <returns>True if element is clickable</returns>
        public bool WaitForElementClickable(By locator)
        {
            return WaitForElementClickable(locator, DEFAULT_TIMEOUT);
        }
    }
}