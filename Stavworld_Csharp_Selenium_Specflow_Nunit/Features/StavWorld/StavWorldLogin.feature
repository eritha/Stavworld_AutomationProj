@StavPay
Feature: StavPay Vendor Management
  As a user
  I want to manage vendors in StavPay Dashboard
  So that I can validate vendor information and update vendor details

  @Vendor @Smoke @TestCase1
  Scenario: Validate Active Vendor Count on Dashboard and Vendor Summary
    Given I am on the StavPay Dashboard page
    When I am logged in to the system
    Then I should be successfully logged in
    When I get the active vendor count from Dashboard
    And I navigate to Vendor Summary page
    And I get the active vendor count from Vendor Table row count
    Then the active vendor count on Dashboard should match Vendor Summary count

  @Vendor @Edit @TestCase2
  Scenario: Edit Vendor US Tax Classification
    Given I am on the StavPay Dashboard page
    When I am logged in to the system
    Then I should be successfully logged in
    And I navigate to Vendor Summary page
    And I click on vendor "The River Fund"
    When I click on Edit button
    And I set US Tax Classification to "Partnership"
    And I save the changes
    Then a success message should be displayed
    And the US Tax Classification should be updated to "Partnership"