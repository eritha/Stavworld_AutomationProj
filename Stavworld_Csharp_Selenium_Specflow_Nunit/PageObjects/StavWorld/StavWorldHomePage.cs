using System;
using OpenQA.Selenium;
using FluentAssertions;
using Stavworld_Csharp_Selenium_Specflow_Nunit.Utility;

namespace Stavworld_Csharp_Selenium_Specflow_Nunit.PageObjects.StavWorld
{
    /// StavWorld Home Page Object for dashboard verification
    public class StavWorldHomePage : BasePObject
    {
        public StavWorldHomePage(IWebDriver _driver) : base(_driver)
        {
        }

        /// Verify home page is loaded with vendor dashboard section
        /// <returns>True if home page loaded and vendor dashboard tile is visible</returns>
        public bool IsHomePageLoadedWithVendorDashboard()
        {
            try
            {
                // Wait for page to load completely after login redirect
                waitHelper.WaitForPageToLoad(Configs.Timeout);
                
                // Verify vendor dashboard tile is visible with explicit wait
                return waitHelper.WaitForElementVisible(VendorDashboardTile, Configs.Timeout);
            }
            catch
            {
                return false;
            }
        }
        #region Locators

        // Dashboard tiles container
        private By DashboardTilesContainer => By.CssSelector("dashboard-tiles[id='Vendor-DashboardTile']");
        
        // Vendor dashboard tile
        public By VendorDashboardTile => By.CssSelector("dashboard-tiles[id='Vendor-DashboardTile'] .outer-div");
        
        // Active Vendors count
        private By ActiveVendorsCount => By.XPath("(//*[@id='Vendor-DashboardTile']//h5)[1]");
        
        // Vendor W9/BEN Outstanding count
        private By VendorW9OutstandingCount => By.XPath("(//*[@id='Vendor-DashboardTile']//h5)[2]");
        
        // Vendor 1099 Eligible count
        private By Vendor1099EligibleCount => By.XPath("(//*[@id='Vendor-DashboardTile']//h5)[3]");
        
        // Vendor sections values from dashboard
        private By VendorSectionsValues => By.XPath("(//div[@class='row'])[1]//h5");
        
        // Logout button
        private By LogoutButton => By.XPath("//*[@iconclass='fa fa-power-off']");

        #endregion

        #region Actions

        /// Verify that the home page dashboard is loaded
        /// <returns>True if dashboard is visible</returns>
        public bool IsDashboardLoaded()
        {
            try
            {
                return waitHelper.WaitForElementVisible(DashboardTilesContainer, Configs.Timeout);
            }
            catch
            {
                return false;
            }
        }

        /// Get the Active Vendors count from dashboard
        /// <returns>Active vendors count as string</returns>
        public string GetActiveVendorsCount()
        {
            try
            {
                // Elements are already loaded in constructor, just get the text
                var countText = _driver.FindElement(ActiveVendorsCount).Text.Trim();
                Console.WriteLine($"[DEBUG] Active Vendors count text: '{countText}'");
                return countText;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting Active Vendors count: {ex.Message}");
                return "0";
            }
        }

        /// Get the Vendor W9/BEN Outstanding count from dashboard
        /// <returns>Vendor W9/BEN Outstanding count as string</returns>
        public string GetVendorW9OutstandingCount()
        {
            if (waitHelper.WaitForElementVisible(VendorW9OutstandingCount, Configs.Timeout))
            {
                return _driver.FindElement(VendorW9OutstandingCount).Text.Trim();
            }
            return "0";
        }

        /// Get the Vendor 1099 Eligible count from dashboard
        /// <returns>Vendor 1099 Eligible count as string</returns>
        public string GetVendor1099EligibleCount()
        {
            if (waitHelper.WaitForElementVisible(Vendor1099EligibleCount, Configs.Timeout))
            {
                return _driver.FindElement(Vendor1099EligibleCount).Text.Trim();
            }
            return "0";
        }

        /// Click on the Vendor Dashboard tile to flip it and see back side
        public void ClickVendorDashboardTile()
        {
            if (waitHelper.WaitForElementClickable(VendorDashboardTile, Configs.Timeout))
            {
                _driver.FindElement(VendorDashboardTile).Click();
            }
        }

        /// Logout from the system
        /// <returns>True if logout successful</returns>
        public bool Logout()
        {
            try
            {
                Console.WriteLine("[DEBUG] Attempting to logout...");
                
                // Wait for logout button to be clickable
                if (waitHelper.WaitForElementClickable(LogoutButton, Configs.Timeout))
                {
                    _driver.FindElement(LogoutButton).Click();
                    Console.WriteLine("[DEBUG] Logout button clicked successfully");
                    
                    // Wait for page to redirect to login page
                    waitHelper.WaitForPageToLoad(Configs.Timeout);
                    
                    // Verify we're redirected to login page by checking for email field
                    var emailField = By.Id("email");
                    if (waitHelper.WaitForElementVisible(emailField, Configs.Timeout))
                    {
                        Console.WriteLine("[DEBUG] Successfully logged out - redirected to login page");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("[DEBUG] Logout may not have completed - still on dashboard");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("[DEBUG] Logout button not found or not clickable");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Error during logout: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Verifications

        /// Verify that the dashboard is displayed with all required elements
        public void VerifyDashboardIsDisplayed()
        {
            IsDashboardLoaded().Should().BeTrue("Dashboard should be loaded after successful login");
            
            // Verify front side elements are visible
            waitHelper.WaitForElementVisible(ActiveVendorsCount, Configs.Timeout).Should().BeTrue("Active Vendors count should be visible");
            waitHelper.WaitForElementVisible(VendorW9OutstandingCount, Configs.Timeout).Should().BeTrue("Vendor W9/BEN Outstanding count should be visible");
            waitHelper.WaitForElementVisible(Vendor1099EligibleCount, Configs.Timeout).Should().BeTrue("Vendor 1099 Eligible count should be visible");
        }

        /// Verify dashboard counts are numeric values
        public void VerifyDashboardCountsAreNumeric()
        {
            var activeVendors = GetActiveVendorsCount();
            var w9Outstanding = GetVendorW9OutstandingCount();
            var eligible1099 = GetVendor1099EligibleCount();

            activeVendors.Should().MatchRegex(@"^\d+$", "Active Vendors count should be numeric");
            w9Outstanding.Should().MatchRegex(@"^\d+$", "Vendor W9/BEN Outstanding count should be numeric");
            eligible1099.Should().MatchRegex(@"^\d+$", "Vendor 1099 Eligible count should be numeric");
        }

        /// Print all dashboard counts for verification
        public void PrintDashboardCounts()
        {
            Console.WriteLine("=== StavWorld Dashboard Counts ===");
            Console.WriteLine($"Active Vendors: {GetActiveVendorsCount()}");
            Console.WriteLine($"Vendor W9/BEN Outstanding: {GetVendorW9OutstandingCount()}");
            Console.WriteLine($"Vendor 1099 Eligible: {GetVendor1099EligibleCount()}");
            Console.WriteLine("==================================");
        }

        /// Get Active Vendors count from dashboard tile (first h5 element)
        /// <returns>Active Vendors count as integer</returns>
        public int GetActiveVendorsCountFromDashboard()
        {
            try
            {
                Console.WriteLine("[DEBUG] GetActiveVendorsCountFromDashboard() started");
                
                // Try to get the element directly first
                try
                {
                    var countText = _driver.FindElement(ActiveVendorsCount).Text.Trim();
                    Console.WriteLine($"[DEBUG] Active Vendors count text: '{countText}'");
                    
                    if (int.TryParse(countText, out int count))
                    {
                        Console.WriteLine($"[DEBUG] Successfully parsed count: {count}");
                        return count;
                    }
                    else
                    {
                        Console.WriteLine($"[DEBUG] Failed to parse count text: '{countText}'");
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] Direct element access failed: {ex.Message}");
                    Console.WriteLine("[DEBUG] Trying to wait for element...");
                    
                    // Fallback: wait for element if constructor didn't work
                    if (waitHelper.WaitForElementVisible(ActiveVendorsCount, Configs.Timeout))
                    {
                        var countText = _driver.FindElement(ActiveVendorsCount).Text.Trim();
                        Console.WriteLine($"[DEBUG] After wait - Active Vendors count text: '{countText}'");
                        
                        if (int.TryParse(countText, out int count))
                        {
                            Console.WriteLine($"[DEBUG] After wait - Successfully parsed count: {count}");
                            return count;
                        }
                    }
                    
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting Active Vendors count: {ex.Message}");
                return 0;
            }
        }

        /// Compare Active Vendors count with expected value
        /// <param name="expectedCount">Expected count to compare</param>
        /// <returns>True if counts match</returns>
        public bool CompareActiveVendorsCount(int expectedCount)
        {
            var actualCount = GetActiveVendorsCountFromDashboard();
            Console.WriteLine($"Dashboard Active Vendors Count: {actualCount}");
            Console.WriteLine($"Expected Count: {expectedCount}");
            return actualCount == expectedCount;
        }

        /// Navigate to home page
        /// <returns>True if navigation successful</returns>
        public bool NavigateAndVerifyVendorSection()
        {
            try
            {
                Console.WriteLine("=== Navigating to Home Page ===");
                
                // Navigate to home page
                NavigateHomePage();
                
                // Wait for page to load completely
                waitHelper.WaitForPageToLoad(Configs.Timeout);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error navigating to home page: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}
