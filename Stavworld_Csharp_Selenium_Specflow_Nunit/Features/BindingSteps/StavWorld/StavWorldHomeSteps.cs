using System;
using TechTalk.SpecFlow;
using FluentAssertions;
using OpenQA.Selenium;
using BoDi;
using Stavworld_Csharp_Selenium_Specflow_Nunit.PageObjects.StavWorld;
using Stavworld_Csharp_Selenium_Specflow_Nunit.Utility;
using Stavworld_Csharp_Selenium_Specflow_Nunit.Base;

namespace Stavworld_Csharp_Selenium_Specflow_Nunit.Features.BindingSteps.StavWorld
{
    /// Step definitions for StavWorld Login functionality
    [Binding]
    public class StavWorldHomeSteps
    {
        private readonly IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;
        private StavWorldLoginPage _loginPage;
        private StavWorldHomePage _homePage;

        public StavWorldHomeSteps(IObjectContainer container)
        {
            driver = container.Resolve<IWebDriver>();
            _scenarioContext = container.Resolve<ScenarioContext>();
        }

        [Then(@"I should be successfully logged in")]
        [Given(@"I should be successfully logged in")]
        public void ShouldBeSuccessfullyLoggedIn()
        {
            // Initialize login page if not already done
            if (_loginPage == null)
            {
                _loginPage = new StavWorldLoginPage(driver);
            }
            
            // Initialize home page if not already done
            if (_homePage == null)
            {
                _homePage = new StavWorldHomePage(driver);
            }
            
            // Wait for login redirect to complete
            _loginPage.WaitForPageToLoad(Configs.Timeout);
            
            // Check if we're still on login page (login failed)
            var isLoginPageStillVisible = _loginPage.IsLoginFormVisible();
            if (isLoginPageStillVisible)
            {
                Console.WriteLine("❌ Login failed - still on login page");
                Console.WriteLine($"Current URL: {driver.Url}");
                Console.WriteLine($"Page Title: {driver.Title}");
                
                // Check for error messages
                var errorMessage = _loginPage.GetErrorMessage();
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine($"Login Error Message: {errorMessage}");
                }
                
                throw new Exception("Login failed - still on login page. Check credentials and error messages.");
            }
            
            // Verify we're on the dashboard
            var isDashboardLoaded = _homePage.IsHomePageLoadedWithVendorDashboard();
            isDashboardLoaded.Should().BeTrue("Login should be successful and dashboard should be loaded");
            
            Console.WriteLine("✅ Successfully logged in and redirected to dashboard");
        }

        [Then(@"I should see the StavWorld home page dashboard")]
        public void ThenIShouldSeeTheStavWorldHomePageDashboard()
        {
            // Verify vendor dashboard tile is visible
            var isVendorDashboardVisible = _homePage.waitHelper.WaitForElementVisible(_homePage.VendorDashboardTile, Configs.Timeout);
            isVendorDashboardVisible.Should().BeTrue("Vendor dashboard tile should be visible");
            
            // Get Active Vendors count from dashboard
            var activeVendorsCount = _homePage.GetActiveVendorsCountFromDashboard();
            Console.WriteLine($"Active Vendors Count from Dashboard: {activeVendorsCount}");
            
            // Verify dashboard is displayed
            _homePage.VerifyDashboardIsDisplayed();
            
            // Verify counts are numeric
            _homePage.VerifyDashboardCountsAreNumeric();
            
            // Print dashboard counts for verification
            _homePage.PrintDashboardCounts();
        }

        [When(@"I get the active vendor count from Dashboard")]
        [Then(@"I get the active vendor count from Dashboard")]
        [Given(@"I get the active vendor count from Dashboard")]
        public void GetTheActiveVendorCountFromDashboard()
        {
            // Initialize home page if not already done
            if (_homePage == null)
            {
                _homePage = new StavWorldHomePage(driver);
            }
            
            // Get Active Vendors count from dashboard 
            var activeVendorsCount = _homePage.GetActiveVendorsCountFromDashboard();
            Console.WriteLine($"Active Vendors Count from Dashboard: {activeVendorsCount}");
            
            // Store the count in scenario context for later comparison
            _scenarioContext["DashboardActiveVendorCount"] = activeVendorsCount;
        }


        [Then(@"I logout from the system")]
        [Given(@"I logout from the system")]
        public void LogoutFromTheSystem()
        {
            // Initialize home page if not already done
            if (_homePage == null)
            {
                _homePage = new StavWorldHomePage(driver);
            }
            
            var logoutResult = _homePage.Logout();
            logoutResult.Should().BeTrue("Logout should be successful");
            
            Console.WriteLine("Successfully logged out from StavPay Dashboard");
        }
    }
}