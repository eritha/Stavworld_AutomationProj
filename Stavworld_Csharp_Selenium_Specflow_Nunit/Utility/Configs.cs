using NUnit.Framework;
using System;
using System.Configuration;

public enum ConfigKeys
{
    ENVIRONMENT,
    QC_NAME,
    ORGANIZATION_NAME,
    SELENIUM_HUB,
    SELENIUM_BASE_URL,
    BROWSER,
    REPORT_HTLM,
}

namespace Stavworld_Csharp_Selenium_Specflow_Nunit.Utility
{
    public class Configs
    {
        public static string varEnvironment;
        public static string varQcName;
        public static string varOrgName;
        public static string varSeleniumHub;
        public static string varSeleniumBaseUrl;
        public static string varBrowser;
        public static string varReportHtlm;

        // Timeout Settings
        public static int Timeout;

        /// Get any parameter from TestContext of nunit.runsettings, if not get AppSettings key from app.config
        /// <param name="para">The parameter name to retrieve</param>
        /// <returns>Configuration value as string</returns>
        static string GetConfigValue(string para)
        {
            var value = "";
            try
            {
                value = TestContext.Parameters[para] != null ? TestContext.Parameters[para] : ConfigurationManager.AppSettings[para];
            }
            catch (SettingsPropertyNotFoundException ex)
            {
                Console.WriteLine("Parameter NOT found : " + para);
                Console.WriteLine("Class FileUtils | Method GetParameter | Exception desc : " + ex.Message);
            }
            return value;
        }

        /// Get a value from nunit.runsettings|app.config with Environment
        /// <param name="keyName">The key name to retrieve with environment prefix</param>
        /// <returns>Configuration value as string</returns>
        public static string GetConfigValueByEnvironment(string keyName)
        {
            var value = "";
            try
            {
                value = GetConfigValue(string.Join("_", varEnvironment, keyName));
            }
            catch (SettingsPropertyNotFoundException ex)
            {
                Console.WriteLine("Parameter NOT found : " + string.Join(varEnvironment, "_", keyName));
                Console.WriteLine("Class FileUtils | Method GetParameter | Exception desc : " + ex.Message);
            }
            return value;
        }
        /// Initialize all data configuration from nunit.runsettings|app.config
        public static void InitDataConfig()
        {
            varEnvironment = GetConfigValue(ConfigKeys.ENVIRONMENT.ToString());
            varQcName = GetConfigValueByEnvironment(ConfigKeys.QC_NAME.ToString());
            varOrgName = GetConfigValueByEnvironment(ConfigKeys.ORGANIZATION_NAME.ToString());
            varSeleniumHub = GetConfigValueByEnvironment(ConfigKeys.SELENIUM_HUB.ToString());
            varSeleniumBaseUrl = GetConfigValueByEnvironment(ConfigKeys.SELENIUM_BASE_URL.ToString());
            varBrowser = GetConfigValueByEnvironment(ConfigKeys.BROWSER.ToString());
            varReportHtlm = GetConfigValueByEnvironment(ConfigKeys.REPORT_HTLM.ToString());
            
            // Load timeout value
            var timeoutValue = GetConfigValue("TIMEOUT");
            Timeout = int.TryParse(timeoutValue, out int timeout) ? timeout : 15; // Default to 15 if parsing fails
        }
    }
}