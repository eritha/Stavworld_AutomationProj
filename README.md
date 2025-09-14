# StavPay Dashboard Automation Testing Framework - Using C#.NET, SpecFlow, Selenium, NUnit, Extent Report
## Overview

```
This project demonstrates a comprehensive automation testing framework for the StavPay Dashboard (https://stavpaydemo2.stavtar.com/layout/dashboard).
It showcases best practices in test automation using C#.NET, SpecFlow, Selenium WebDriver, and NUnit with Extent Reports.
The framework implements the Page Object Model design pattern for scalable, maintainable, readable, and repeatable test automation.
```

**Resources**
- [Selenium](https://www.guru99.com/page-object-model-pom-page-factory-in-selenium-ultimate-guide.html)
- [Selenium](http://www.seleniumhq.org/)
- [SpecFlow](http://specflow.org/)
- [Nunit](https://nunit.org/)
- [Extent Report](https://extentreports.com/)
- [FluentAssertions](https://fluentassertions.com/)

## StavPay Dashboard Demo Features:

### 🔐 **Complete StavPay Dashboard Testing**
- **StavPay Dashboard**: https://stavpaydemo2.stavtar.com/layout/dashboard
- **User Authentication**: Login with valid credentials
- **Dashboard Verification**: Home page and vendor section validation
- **Vendor Management**: Active vendor count validation and comparison
- **Vendor Editing**: US Tax Classification update functionality

### 🎯 **Test Scenarios Covered**

#### **@Vendor @Smoke @TestCase1**
- ✅ **Validate Active Vendor Count on Dashboard and Vendor Summary**
  - Given I am on the StavPay Dashboard page
  - And I am logged in to the system
  - And I get the active vendor count from Dashboard
  - When I navigate to Vendor Summary page
  - And I get the active vendor count from Vendor Summary
  - Then the active vendor count on Dashboard should match Vendor Summary count

#### **@Vendor @Edit @TestCase2**
- ✅ **Edit Vendor US Tax Classification**
  - Given I am on the StavPay Dashboard page
  - And I am logged in to the system
  - When I navigate to Vendor Summary page
  - And I click on vendor "The River Fund"
  - And I click on Edit button
  - And I set US Tax Classification to "Partnership"
  - And I save the changes
  - Then a success message should be displayed
  - Then the US Tax Classification should be updated to "Partnership"

### 🚀 **Quick Start**
- **Run Demo**: Use `RunStavWorldDemo.bat` for quick execution
- **Login Credentials**: abc@str.com / Abc@123456
- **Test Categories**: StavPay, Vendor, Smoke, Edit, TestCase1, TestCase2

## Pre-requisites

#### Condition: No additional setup required for StavPay Dashboard demo

#### Tools & Libs:

* Visual Studio: 2017 or later
* .NET Framework: 4.8
* SpecFlow: 3.9.74
* SpecFlow.NUnit: 3.9.74
* SpecFlow.Tools.MsBuild.Generation: 3.9.74
* NUnit: 3.13.3
* NUnit3TestAdapter: 4.3.1
* Microsoft.NET.Test.Sdk: 17.6.0
* Selenium.WebDriver: 4.15.0
* Selenium.Support: 4.15.0
* WebDriverManager: 2.17.4
* DotNetSeleniumExtras.WaitHelpers: 3.11.0
* ExtentReports: 5.0.4
* FluentAssertions: 6.12.0
* BoDi: 1.5.0
* Newtonsoft.Json: 13.0.3

### Visual Studio

Visual Studio needs a little extra configuration. Install these extensions:
- SpecFlow for Visual studio:
	+ Extension for VS 2015: https://marketplace.visualstudio.com/items?itemName=TechTalkSpecFlowTeam.SpecFlowforVisualStudio2015
	+ Extension for VS 2017: https://marketplace.visualstudio.com/items?itemName=TechTalkSpecFlowTeam.SpecFlowforVisualStudio2017
- Or install throught IDE:
	+ select “Tools”, “Extensions and Updates…” from the menu. Click on "Online" on the left, and search for "SpecFlow for Visual studio", then click "Download"

## Running Tests
You can run them in the Unit Test explorer on Visual Studio or via command line

Build
```
$ dotnet build
```

### IDE - Visual Studio

When you build the test project, the tests appear in Test Explorer. 
If Test Explorer is not visible, choose Test on the Visual Studio menu, choose Windows, and then choose Test Explorer.
+ Right-click menu for a selected test and then choose Run Selected Tests.

### Command Line


#### Run StavPay Dashboard Demo specifically
```
$ RunStavWorldDemo.bat
```

#### Run all tests at .sln directory
```
$ dotnet test
```

Run tests by Category
```
$ dotnet test --filter "Category=Vendor"
$ dotnet test --filter "Category=Smoke"
$ dotnet test --filter "Category=Edit"
$ dotnet test --filter "Category=TestCase1"
$ dotnet test --filter "Category=TestCase2"
```

Run StavPay tests by category
```
$ dotnet test --filter "Category=StavPay"
```

Run tests with parameters
```
$ dotnet test --filter "Category=Vendor" -- --browser=Chrome
```