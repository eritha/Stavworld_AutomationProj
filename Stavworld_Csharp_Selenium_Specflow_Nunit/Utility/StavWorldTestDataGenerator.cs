using System;

namespace Stavworld_Csharp_Selenium_Specflow_Nunit.Utility
{
    /// Test data generator for StavWorld login
    public static class StavWorldTestDataGenerator
    {
        /// Generate valid login credentials
        /// <returns>Valid login credentials</returns>
        public static (string Email, string Password) GenerateValidCredentials()
        {
            return ("abc@sstv.com", "Abc@123123");
        }
    }
}