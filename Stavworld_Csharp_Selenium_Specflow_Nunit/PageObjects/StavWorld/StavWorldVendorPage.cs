using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using FluentAssertions;
using Stavworld_Csharp_Selenium_Specflow_Nunit.Utility;

namespace Stavworld_Csharp_Selenium_Specflow_Nunit.PageObjects.StavWorld
{
    /// Page Object for StavWorld Vendor Management
    public class StavWorldVendorPage : BasePObject
    {
        // StavPay Dashboard elements
        private readonly By _vendorSummaryLink = By.XPath("//a[@id='Vendor']");
        private readonly By _dashboardTitle = By.ClassName("dashboard-title");
        private readonly By _mainContent = By.ClassName("main-content");
        
        // Vendor Summary page elements
        private readonly By _vendorTableRowCount = By.XPath("//div[contains(@class,'ag-status-panel-total-and-filtered-row-count')]//span[contains(@class,'ag-status-name-value-value')]");
        private readonly By _vendorTable = By.ClassName("vendor-table");
        private readonly By _agTableContainer = By.CssSelector(".ag-center-cols-container");
        private readonly By _vendorDashboardData = By.XPath("(//div[@class='row'])[1]");
        
        /// Create dynamic XPath locator for vendor by name
        private By GetVendorLocator(string vendorName)
        {
            var xpath = string.Format("//*[@col-id='VendorLongName'][text()='{0}']", vendorName);
            return By.XPath(xpath);
        }
        
        
        // Vendor Detail page elements
        private readonly By _editButton = By.XPath("//*[@class='k-button-icon fal fa-pen ng-star-inserted']/parent::button");
        private readonly By _usTaxClassificationCombobox = By.XPath("//*[@id='Vendor-VendorDetails-DetailsTab-TaxClassification']//input");
        private readonly By _saveButton = By.XPath("//span[normalize-space()='Save']");
        private readonly By _updatedTaxClassification = By.XPath("//div[@id='Vendor-VendorDetails-DetailsTab-TaxClassification']//input");
        
        private readonly By _successMessage = By.XPath("//*[contains(@class, 'toast-message')]");
        private readonly By _successMessageText = By.XPath("//*[contains(text(), 'Data Has Been Updated Successfully!')]");

        public StavWorldVendorPage(IWebDriver _driver) : base(_driver)
        {
        }

        /// Navigate to Vendor Summary page
        public void NavigateToVendorSummary()
        {
            try
            {
                waitHelper.WaitForElementClickable(_vendorSummaryLink, Configs.Timeout);
                
                _driver.FindElement(_vendorSummaryLink).Click();
                waitHelper.WaitForPageToLoad();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error navigating to Vendor Summary: {ex.Message}");
                throw;
            }
        }


        /// Get vendor table row count from Vendor Summary page
        public int GetVendorTableRowCount()
        {
            try
            {
                Console.WriteLine("[DEBUG] Waiting for vendor dashboard data to be published...");
                
                // First wait for the vendor dashboard data to be published
                waitHelper.WaitForElementVisible(_vendorDashboardData, Configs.Timeout);
                Console.WriteLine("[DEBUG] Vendor dashboard data is published");
                
                Console.WriteLine("[DEBUG] Waiting for AG Grid table to load...");
                
                // Then wait for the table container to be visible
                waitHelper.WaitForElementVisible(_agTableContainer, Configs.Timeout);
                Console.WriteLine("[DEBUG] AG Grid table container is visible");
                
                // Wait a bit more for the table to fully load with data
                System.Threading.Thread.Sleep(2000);
                
                // Now wait for the count element to be visible
                waitHelper.WaitForElementVisible(_vendorTableRowCount, Configs.Timeout);
                var countText = _driver.FindElement(_vendorTableRowCount).Text;
                Console.WriteLine($"[DEBUG] Vendor summary count text: '{countText}'");
                
                // If count is empty or "0", wait a bit more and try again
                if (string.IsNullOrEmpty(countText) || countText == "0")
                {
                    Console.WriteLine("[DEBUG] Count is empty or 0, waiting for data to load...");
                    System.Threading.Thread.Sleep(3000);
                    countText = _driver.FindElement(_vendorTableRowCount).Text;
                    Console.WriteLine($"[DEBUG] Retry - Vendor summary count text: '{countText}'");
                }
                
                var count = int.Parse(countText);
                Console.WriteLine($"[DEBUG] Parsed vendor summary count: {count}");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting vendor summary count: {ex.Message}");
                return 0;
            }
        }

        /// Click on specific vendor link
        /// <param name="vendorName">Name of the vendor</param>
        public void ClickOnVendor(string vendorName)
        {
            try
            {
                // Create dynamic XPath locator for the vendor
                var vendorLink = GetVendorLocator(vendorName);
                
                Console.WriteLine($"[DEBUG] Looking for vendor: {vendorName}");
                waitHelper.WaitForElementClickable(vendorLink, Configs.Timeout);
                
                _driver.FindElement(vendorLink).Click();
                Console.WriteLine($"[DEBUG] Successfully clicked on vendor: {vendorName}");
                WaitForPageToLoad();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clicking on vendor {vendorName}: {ex.Message}");
                throw;
            }
        }

        /// Click on Edit button
        public void ClickEditButton()
        {
            try
            {
                waitHelper.WaitForElementClickable(_editButton, Configs.Timeout);
                
                _driver.FindElement(_editButton).Click();
                WaitForPageToLoad();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clicking edit button: {ex.Message}");
                throw;
            }
        }

        /// Set US Tax Classification using combobox with SendKeys and XPath option selection
        /// <param name="taxClassification">Tax classification value</param>
        public void SetUsTaxClassification(string taxClassification)
        {
            try
            {
                Console.WriteLine($"[DEBUG] Setting US Tax Classification to: {taxClassification}");
                
                // Wait for combobox to be visible and clickable
                waitHelper.WaitForElementVisible(_usTaxClassificationCombobox, Configs.Timeout);
                waitHelper.WaitForElementClickable(_usTaxClassificationCombobox, Configs.Timeout);
                
                // Check if clear button (X) exists and click it if present
                var clearButtonLocator = By.XPath("//*[@id='Vendor-VendorDetails-DetailsTab-TaxClassification']//span[@title='clear']");
                try
                {
                    var clearButton = _driver.FindElement(clearButtonLocator);
                    if (clearButton.Displayed && clearButton.Enabled)
                    {
                        Console.WriteLine("[DEBUG] Clear button found, clicking to clear existing value");
                        clearButton.Click();
                        System.Threading.Thread.Sleep(200); // Small delay after clear
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("[DEBUG] Clear button not found, proceeding with normal clear");
                }

                // Clear and send keys to combobox
                var combobox = _driver.FindElement(_usTaxClassificationCombobox);
                
                combobox.Clear();
                combobox.SendKeys(taxClassification);
                
                Console.WriteLine($"[DEBUG] Sent keys to combobox: {taxClassification}");
            
                // Create dynamic XPath for the option
                var optionXPath = string.Format("//span[normalize-space()='{0}']", taxClassification);
                var optionLocator = By.XPath(optionXPath);
                
                Console.WriteLine($"[DEBUG] Looking for option with XPath: {optionXPath}");
                
                // Wait for option to be visible and clickable
                waitHelper.WaitForElementVisible(optionLocator, Configs.Timeout);
                waitHelper.WaitForElementClickable(optionLocator, Configs.Timeout);
                
                // Click on the option
                _driver.FindElement(optionLocator).Click();
                
                Console.WriteLine($"[DEBUG] Successfully selected option: {taxClassification}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting tax classification '{taxClassification}': {ex.Message}");
                throw;
            }
        }

        /// Save vendor changes
        public void SaveChanges()
        {
            try
            {
                Console.WriteLine("[DEBUG] Clicking Save button...");
                waitHelper.WaitForElementClickable(_saveButton, Configs.Timeout);
                
                _driver.FindElement(_saveButton).Click();
                Console.WriteLine("[DEBUG] Save button clicked");
                
                WaitForPageToLoad();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                throw;
            }
        }

        /// Wait for success message to appear after save
        /// <returns>True if success message appears</returns>
        public bool WaitForSuccessMessage()
        {
            try
            {
                Console.WriteLine("[DEBUG] Waiting for success message to appear...");
                waitHelper.WaitForElementVisible(_successMessage, Configs.Timeout);
                
                // Verify the specific success message text
                var successElement = _driver.FindElement(_successMessageText);
                var messageText = successElement.Text.Trim();
                
                Console.WriteLine($"[DEBUG] Success message appeared: '{messageText}'");
                
                return messageText.Contains("Data Has Been Updated Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Error waiting for success message: {ex.Message}");
                return false;
            }
        }


        /// Get updated tax classification value
        public string GetUpdatedTaxClassification()
        {
            try
            {
                waitHelper.WaitForElementVisible(_updatedTaxClassification, Configs.Timeout);
                var element = _driver.FindElement(_updatedTaxClassification);
                
                // For input fields, use GetAttribute("value") instead of Text
                var value = element.GetAttribute("value");
                Console.WriteLine($"[DEBUG] Tax classification input value: '{value}'");
                
                return value ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting updated tax classification: {ex.Message}");
                return string.Empty;
            }
        }

        /// Verify tax classification is updated
        /// <param name="expectedValue">Expected tax classification value</param>
        /// <returns>True if updated correctly</returns>
        public bool IsTaxClassificationUpdated(string expectedValue)
        {
            try
            {
                var actualValue = GetUpdatedTaxClassification();
                return actualValue.Equals(expectedValue, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying tax classification: {ex.Message}");
                return false;
            }
        }

        /// Wait for page to load completely with explicit wait
        /// <param name="timeoutInSeconds">Timeout in seconds</param>
        public void WaitForPageToLoad(int timeoutInSeconds = 30)
        {
            try
            {
                var wait = new WebDriverWait(
                    _driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(_driver => ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState").Equals("complete"));
                
                // Additional wait for dynamic content
                System.Threading.Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Page load wait timeout or error: {ex.Message}");
            }
        }


    }
}
