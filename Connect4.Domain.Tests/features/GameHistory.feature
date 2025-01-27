Feature: View game history
  As a user
  I want to view my finished games
  So that I can see my past performance

  Scenario: Successfully view game history
    Given I am logged in with a valid JWT token
    And I have finished games in my history
    When I send a request to view my game history
    Then I receive a status code 200
    And I receive a list of games with the status "Finished"

  Scenario: View game history with no finished games
    Given I am logged in with a valid JWT token
    And I have no finished games in my history
    When I send a request to view my game history
    Then I receive a status code 200
    And I receive an empty list

  Scenario: View game history while being busy in a game
    Given I am logged in with a valid JWT token
    And I am currently in a game with ID 1 in "InProgress" status
    When I send a request to view my game history
    Then I receive a status code 409
    And I receive an error message "You are busy in game 1, cannot access waiting games."