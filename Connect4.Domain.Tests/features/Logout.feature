Feature: Logout
  As a user
  I want to log out of the application
  So that my JWT token becomes invalid

  Scenario: Successfully log out
    Given I am logged in with a valid JWT token
    When I send a logout request
    Then I receive a status code 200
    And I receive a message "Token has been invalidated (logged out)"

  Scenario: Logout without a valid token
    Given I am not logged in
    When I send a logout request
    Then I receive a status code 401
    And I receive an error message "User is not authenticated"
