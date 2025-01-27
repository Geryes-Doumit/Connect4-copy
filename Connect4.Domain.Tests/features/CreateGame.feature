Feature: Create a new game
  As a user
  I want to create a new Connect4 game
  So that I can wait for another player to join

  Scenario: Successfully create a game
    Given I am logged in with a valid JWT token
    And I provide a valid game name "MyFirstGame"
    When I send a request to create a new game
    Then I receive a status code 200
    And the game is created with the status "WaitingForPlayers"

  Scenario: Create game with invalid name
    Given I am logged in with a valid JWT token
    And I provide an invalid game name ""
    When I send a request to create a new game
    Then I receive a status code 400
    And I receive an error message "Invalid game name"

  Scenario: Create a game while already in another game
    Given I am logged in with a valid JWT token
    And I am already in a game with ID 1
    When I send a request to create a new game
    Then I receive a status code 409
    And I receive an error message "You are already in a game"
