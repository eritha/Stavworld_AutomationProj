@echo off
echo ========================================
echo StavWorld Automation Testing Framework
echo ========================================
echo.

echo Building project...
dotnet build "Stavworld_Csharp_Selenium_Specflow_Nunit.sln"

if %ERRORLEVEL% neq 0 (
    echo Build failed! Please check the errors above.
    pause
    exit /b 1
)

echo.
echo Running StavWorld tests...
echo.

dotnet test "Stavworld_Csharp_Selenium_Specflow_Nunit.sln" --filter "Category=StavWorld" --logger "trx;LogFileName=StavWorldTestResults.trx"

echo.
echo Test execution completed!
echo Check the TestResults folder for detailed reports.
echo.
pause