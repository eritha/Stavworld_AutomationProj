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
    public class StavWorldLoginSteps
    {
        private readonly IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;
        private StavWorldLoginPage _loginPage;
        private StavWorldHomePage _homePage;
        private (string Email, string Password) _credentials;

        public StavWorldLoginSteps(IObjectContainer container)
        {
            driver = container.Resolve<IWebDriver>();
            _scenarioContext = container.Resolve<ScenarioContext>();
        }

        [Given(@"I am on the StavPay Dashboard page")]
        public void GivenIAmOnTheStavPayDashboardPage()
        {
            _loginPage = new StavWorldLoginPage(driver);
            _loginPage.NavigateToLoginPage();
        }

        [When(@"I am logged in to the system")]
        [Given(@"I am logged in to the system")]
        public void AmLoggedInToTheSystem()
        {
            // Initialize login page if not already done
            if (_loginPage == null)
            {
                _loginPage = new StavWorldLoginPage(driver);
            }
            
            // Navigate to login page
            _loginPage.NavigateToLoginPage();
            
            // Generate and enter valid credentials
            _credentials = StavWorldTestDataGenerator.GenerateValidCredentials();
            Console.WriteLine($"Logging in with credentials: {_credentials.Email}");
            
            _loginPage.EnterEmail(_credentials.Email);
            _loginPage.EnterPassword(_credentials.Password);
            
            // Click login button and get home page
            _homePage = _loginPage.ClickLoginButton();
            
            // Verify login was successful by checking if we're on the dashboard
            var isDashboardLoaded = _homePage.IsHomePageLoadedWithVendorDashboard();
            isDashboardLoaded.Should().BeTrue("Login should be successful and dashboard should be loaded");
            
            Console.WriteLine("Successfully logged in to StavPay Dashboard");
        }

        [When(@"I enter valid login credentials")]
        public void WhenIEnterValidLoginCredentials()
        {
            _credentials = StavWorldTestDataGenerator.GenerateValidCredentials();
            _loginPage.EnterEmail(_credentials.Email);
            _loginPage.EnterPassword(_credentials.Password);
        }

        [When(@"I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            _homePage = _loginPage.ClickLoginButton();
        }

    }
}