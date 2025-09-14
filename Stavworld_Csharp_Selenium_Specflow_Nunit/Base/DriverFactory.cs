using Stavworld_Csharp_Selenium_Specflow_Nunit.Utility;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Stavworld_Csharp_Selenium_Specflow_Nunit.Base
{
    public class DriverFactory
    {
        /// Create Selenium Webdriver using WebDriverManager 
        /// <returns>Return IWebDriver</returns>
        public IWebDriver CreateDriver()
        {
            switch (Configs.varBrowser.ToUpperInvariant())
            {
                case "CHROME":
                    return CreateChromeDriver();
                case "FIREFOX":
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    return new FirefoxDriver();
                case "IE":
                    return new InternetExplorerDriver();
                default:
                    return CreateChromeDriver();
            }
        }

        /// Create Chrome driver with optimized settings
        /// <returns>Chrome WebDriver instance</returns>
        private IWebDriver CreateChromeDriver()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var chromeOptions = new ChromeOptions();
            
            // Disable password manager and autofill using user profile preferences
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);
            
            // Essential arguments for CI/CD and containers
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            
            // Disable password save popup
            chromeOptions.AddArgument("--disable-save-password-bubble");
            chromeOptions.AddArgument("--disable-password-manager");
            
            return new ChromeDriver(chromeOptions);
        }
    }
}
