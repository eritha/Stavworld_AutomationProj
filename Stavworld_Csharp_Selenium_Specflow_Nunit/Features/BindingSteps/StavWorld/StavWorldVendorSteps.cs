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
    /// Step definitions for StavWorld Vendor Management feature
    [Binding]
    public class StavWorldVendorSteps
    {
        private readonly IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;
        private StavWorldVendorPage _vendorPage;

        public StavWorldVendorSteps(IObjectContainer container)
        {
            driver = container.Resolve<IWebDriver>();
            _scenarioContext = container.Resolve<ScenarioContext>();
        }

        [When(@"I navigate to Vendor Summary page")]
        [Given(@"I navigate to Vendor Summary page")]
        [Then(@"I navigate to Vendor Summary page")]
        public void NavigateToVendorSummaryPage()
        {
            _vendorPage = new StavWorldVendorPage(driver);
            _vendorPage.NavigateToVendorSummary();
        }
 
        [When(@"I get the active vendor count from Vendor Table row count")]
        [Then(@"I get the active vendor count from Vendor Table row count")]
        public void GetTheActiveVendorCountFromVendorTableRowCount()
        {
            // Initialize vendor page if not already done
            if (_vendorPage == null)
            {
                _vendorPage = new StavWorldVendorPage(driver);
            }
            
            // Get Active Vendors count from dashboard (stored in scenario context)
            var activeVendorsCount = _scenarioContext.Get<int>("DashboardActiveVendorCount");
            Console.WriteLine($"Dashboard Active Vendors Count: {activeVendorsCount}");
            
            // Get Vendor Table Row Count
            var tableRowCount = _vendorPage.GetVendorTableRowCount();
            Console.WriteLine($"Vendor Table Row Count: {tableRowCount}");
            
            // Verify dashboard count is equal to table row count
            Console.WriteLine($"=== COMPARISON RESULT ===");
            Console.WriteLine($"Dashboard Count: {activeVendorsCount}");
            Console.WriteLine($"Table Row Count: {tableRowCount}");
            Console.WriteLine($"Counts Match: {activeVendorsCount == tableRowCount}");
            Console.WriteLine($"=========================");
            
            // Store the count in scenario context for later comparison
            _scenarioContext["VendorTableRowCount"] = tableRowCount;
        }

        [Then(@"the active vendor count on Dashboard should match Vendor Summary count")]
        public void ThenTheActiveVendorCountOnDashboardShouldMatchVendorSummaryCount()
        {
            // Get counts from scenario context
            var dashboardCount = _scenarioContext.Get<int>("DashboardActiveVendorCount");
            var vendorSummaryCount = _scenarioContext.Get<int>("VendorTableRowCount");
            
            Console.WriteLine($"=== COMPARISON RESULT ===");
            Console.WriteLine($"Dashboard Count: {dashboardCount}");
            Console.WriteLine($"Vendor Summary Count: {vendorSummaryCount}");
            Console.WriteLine($"Counts Match: {dashboardCount == vendorSummaryCount}");
            Console.WriteLine($"=========================");
            
            // Assert that counts match
            dashboardCount.Should().Be(vendorSummaryCount, 
                $"Dashboard Active Vendors Count ({dashboardCount}) should match Vendor Summary Count ({vendorSummaryCount})");
        }

        [Given(@"I click on vendor ""(.*)""")]
        [Then(@"I click on vendor ""(.*)""")]
        public void ClickOnVendor(string vendorName)
        {
            _vendorPage.ClickOnVendor(vendorName);
        }

        [When(@"I click on Edit button")]
        [Then(@"I click on Edit button")]
        public void ClickOnEditButton()
        {
            _vendorPage.ClickEditButton();
        }

        [When(@"I set US Tax Classification to ""(.*)""")]
        [Given(@"I set US Tax Classification to ""(.*)""")]
        [Then(@"I set US Tax Classification to ""(.*)""")]
        public void SetUsTaxClassificationTo(string taxClassification)
        {
            _vendorPage.SetUsTaxClassification(taxClassification);
        }

        [When(@"I save the changes")]
        [Given(@"I save the changes")]
        [Then(@"I save the changes")]
        public void SaveTheChanges()
        {
            _vendorPage.SaveChanges();
        }

        [Then(@"the US Tax Classification should be updated to ""(.*)""")]
        [Given(@"the US Tax Classification should be updated to ""(.*)""")]
        public void TheUsTaxClassificationShouldBeUpdatedTo(string expectedTaxClassification)
        {
            var isUpdated = _vendorPage.IsTaxClassificationUpdated(expectedTaxClassification);
            isUpdated.Should().BeTrue($"US Tax Classification should be updated to {expectedTaxClassification}");
        }

        [Then(@"a success message should be displayed")]
        public void ThenASuccessMessageShouldBeDisplayed()
        {
            Console.WriteLine("=== SUCCESS MESSAGE VERIFICATION ===");
            
            // Use the existing WaitForSuccessMessage method
            var messageAppeared = _vendorPage.WaitForSuccessMessage();
            messageAppeared.Should().BeTrue("Success message should appear after saving changes");
            
            Console.WriteLine("Success message verification completed");
            Console.WriteLine("=====================================");
        }
    }
}
