using Stavworld_Csharp_Selenium_Specflow_Nunit.Utility;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Configuration;

namespace Stavworld_Csharp_Selenium_Specflow_Nunit.PageObjects
{
    public abstract class BasePObject
    {
        // Get selenium base URL from app config(default) or run command line with Nunit parameters
        protected string SeleniumBaseUrl = Configs.varSeleniumBaseUrl;

        public WaitHelper waitHelper;
        protected readonly IWebDriver _driver;

        public BasePObject(IWebDriver driver)
        {
            _driver = driver;
            waitHelper = new WaitHelper(driver);
        }

        /// Action: Navigate to Home page from any pages
        public void NavigateHomePage()
        {
            _driver.Navigate().GoToUrl(SeleniumBaseUrl);
            waitHelper.WaitForPageToLoad();
        }

        /// Action: Navigate from any pages to the page <url>
        public void Navigate(string url)
        {
            _driver.Navigate().GoToUrl(url);
            waitHelper.WaitForPageToLoad();
        }

        /// Action: Navigate to URL with custom timeout
        public void NavigateToUrl(string url, int timeoutInSeconds = 30)
        {
            _driver.Navigate().GoToUrl(url);
            waitHelper.WaitForPageToLoad(timeoutInSeconds);
        }

        /// Verify: The current page title <title>
        public void AssertTitle(string title)
        {
            string pageTitle = _driver.Title;
            pageTitle.Should().Be(title);
        }

        /// Verify: The current page contains title <title>
        public void AssertContainsTitle(string title)
        {
            string pageTitle = _driver.Title;
            pageTitle.Should().Contain(title);
        }
    }
}