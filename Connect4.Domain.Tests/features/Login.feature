Feature: User Login
  As a user
  I want to log in to the application
  So that I can access my account and start playing

  Scenario: Successful login
    Given I am a user with a valid username "Marc" and password "marc"
    When I send a login request
    Then I receive a status code 200
    And I receive a JWT token

  Scenario: Login with invalid credentials
    Given I am a user with an invalid username or password
    When I send a login request
    Then I receive a status code 401
    And I receive an error message "Invalid username or password"

  Scenario: Missing Authorization header
    Given I am a user without an Authorization header
    When I send a login request
    Then I receive a status code 400
    And I receive an error message "Missing Authorization header"
