Feature: Leave a game
  As a player
  I want to leave an active or waiting game
  So that I can quit the game session

  Scenario: Successfully leave a waiting game
    Given I am logged in with a valid JWT token
    And I am the host of a game with ID 1 in "WaitingForPlayers" status
    When I send a request to leave the game
    Then I receive a status code 200
    And the game is deleted

  Scenario: Successfully leave an in-progress game
    Given I am logged in with a valid JWT token
    And I am a player in a game with ID 2 in "InProgress" status
    When I send a request to leave the game
    Then I receive a status code 200
    And the game status is updated to "Finished"
    And the other player is declared the winner

  Scenario: Leave a game that does not exist
    Given I am logged in with a valid JWT token
    And there is no game with ID 99
    When I send a request to leave the game with ID 99
    Then I receive a status code 404
    And I receive an error message "Game not found"

  Scenario: Leave a game as a non-participant
    Given I am logged in with a valid JWT token
    And I am not a participant in a game with ID 1
    When I send a request to leave the game
    Then I receive a status code 403
    And I receive an error message "You are not authorized to leave this game"
