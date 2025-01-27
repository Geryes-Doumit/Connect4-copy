Feature: Join an existing game
  As a user
  I want to join a game that is waiting for players
  So that I can start playing

  Scenario: Successfully join a game
    Given I am logged in with a valid JWT token
    And there is a game with ID 1 in "WaitingForPlayers" status
    When I send a request to join the game with ID 1
    Then I receive a status code 200
    And I am added to the game as the guest player

  Scenario: Join a game that is already full
    Given I am logged in with a valid JWT token
    And the game with ID 1 already has both players
    When I send a request to join the game with ID 1
    Then I receive a status code 409
    And I receive an error message "Game is already full"

  Scenario: Join a game that does not exist
    Given I am logged in with a valid JWT token
    And there is no game with ID 99
    When I send a request to join the game with ID 99
    Then I receive a status code 404
    And I receive an error message "Game not found"
