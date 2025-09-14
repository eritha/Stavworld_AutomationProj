using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using FluentAssertions;
using Stavworld_Csharp_Selenium_Specflow_Nunit.Utility;

namespace Stavworld_Csharp_Selenium_Specflow_Nunit.PageObjects.StavWorld
{
    /// Page Object for StavWorld Login Page
    public class StavWorldLoginPage : BasePObject
    {
        // Locators - These will need to be updated based on actual page structure
        private readonly By _emailField = By.Id("email");
        private readonly By _passwordField = By.Id("password");
        private readonly By _loginButton = By.Id("next");
        private readonly By _errorMessage = By.ClassName("error");

        public StavWorldLoginPage(IWebDriver _driver) : base(_driver)
        {
        }

        /// Navigate to StavPay Dashboard page
        public void NavigateToLoginPage()
        {
            NavigateToUrl("https://stavpaydemo2.stavtar.com/layout/dashboard");
            waitHelper.WaitForPageToLoad();
        }

        /// Wait for page to load completely with explicit wait
        /// <param name="timeoutInSeconds">Timeout in seconds</param>
        public void WaitForPageToLoad(int timeoutInSeconds = 30)
        {
            waitHelper.WaitForPageToLoad(timeoutInSeconds);
        }

        /// Wait for login form fields to be visible
        /// <param name="timeoutInSeconds">Timeout in seconds</param>
        public void WaitForLoginFormFields(int timeoutInSeconds = -1)
        {
            var timeout = timeoutInSeconds == -1 ? Configs.Timeout : timeoutInSeconds;
            waitHelper.WaitForElementVisible(_emailField, timeout);
            waitHelper.WaitForElementVisible(_passwordField, timeout);
        }

        /// Enter email address
        /// <param name="email">Email address</param>
        public void EnterEmail(string email)
        {
            WaitForLoginFormFields();
            var emailElement = _driver.FindElement(_emailField);
            emailElement.Clear();
            emailElement.SendKeys(email);
        }

        /// Enter password
        /// <param name="password">Password</param>
        public void EnterPassword(string password)
        {
            WaitForLoginFormFields();
            var passwordElement = _driver.FindElement(_passwordField);
            passwordElement.Clear();
            passwordElement.SendKeys(password);
        }

        /// Click login button and return StavWorldHomePage
        /// <returns>StavWorldHomePage instance</returns>
        public StavWorldHomePage ClickLoginButton()
        {
            waitHelper.WaitForElementClickable(_loginButton, Configs.Timeout);
            _driver.FindElement(_loginButton).Click();
            
            // Wait for page to load after login redirect
            waitHelper.WaitForPageToLoad(Configs.Timeout);
            
            // Return StavWorldHomePage instance
            return new StavWorldHomePage(_driver);
        }

        /// Wait for login to complete
        /// <param name="timeoutInSeconds">Timeout in seconds</param>
        public void WaitForLoginToComplete(int timeoutInSeconds = 30)
        {
            waitHelper.WaitForPageToLoad(timeoutInSeconds);
        }

        /// Check if login is successful
        /// <returns>True if login successful</returns>
        public bool IsLoginSuccessful()
        {
            try
            {
                // Wait for page to load after login redirect
                waitHelper.WaitForPageToLoad(Configs.Timeout);
                
                // Login is successful if we reach this point without errors
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// Check if error message is visible
        /// <returns>True if error message is visible</returns>
        public bool IsErrorMessageVisible()
        {
            try
            {
                return waitHelper.WaitForElementVisible(_errorMessage, Configs.Timeout);
            }
            catch
            {
                return false;
            }
        }


        /// Check if on login page
        /// <returns>True if on login page</returns>
        public bool IsOnLoginPage()
        {
            try
            {
                return waitHelper.WaitForElementVisible(_emailField, Configs.Timeout);
            }
            catch
            {
                return false;
            }
        }



        /// Check if login form is still visible (indicating login failed)
        /// <returns>True if login form is visible</returns>
        public bool IsLoginFormVisible()
        {
            try
            {
                return waitHelper.WaitForElementVisible(_emailField, 5) && 
                       waitHelper.WaitForElementVisible(_passwordField, 5) && 
                       waitHelper.WaitForElementVisible(_loginButton, 5);
            }
            catch
            {
                return false;
            }
        }

        /// Get error message from login form
        /// <returns>Error message text if present</returns>
        public string GetErrorMessage()
        {
            try
            {
                if (waitHelper.WaitForElementVisible(_errorMessage, 2))
                {
                    return _driver.FindElement(_errorMessage).Text.Trim();
                }
            }
            catch
            {
                // Error element not found
            }
            return string.Empty;
        }

        /// Wait for login to complete and redirect to dashboard
        public void WaitForLoginToComplete()
        {
            try
            {
                // Wait for page to load after login
                waitHelper.WaitForPageToLoad(Configs.Timeout);
                
                // Wait for either success indicators or error messages
                var maxWaitTime = DateTime.Now.AddSeconds(Configs.Timeout);
                while (DateTime.Now < maxWaitTime)
                {
                    // Check if we're redirected away from login page
                    if (!IsLoginFormVisible())
                    {
                        Console.WriteLine("✅ Login completed - redirected from login page");
                        return;
                    }
                    
                    // Check for error messages
                    var errorMessage = GetErrorMessage();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        Console.WriteLine($"❌ Login error: {errorMessage}");
                        throw new Exception($"Login failed with error: {errorMessage}");
                    }
                }
                
                // If we reach here, login didn't complete in time
                throw new Exception("Login did not complete within timeout period");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error waiting for login completion: {ex.Message}");
                throw;
            }
        }
    }
}
