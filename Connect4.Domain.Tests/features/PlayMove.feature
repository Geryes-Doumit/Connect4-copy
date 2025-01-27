Feature: Play a move in a game
  As a player
  I want to play a move in my active game
  So that I can advance the game and try to win

  Scenario: Successfully play a move
    Given I am logged in with a valid JWT token
    And I am in a game with ID 1 in "InProgress" status
    And it is my turn to play
    When I send a request to play a move in column 3
    Then I receive a status code 200
    And the game board is updated with my move

  Scenario: Play a move when it is not my turn
    Given I am logged in with a valid JWT token
    And I am in a game with ID 1 in "InProgress" status
    And it is not my turn to play
    When I send a request to play a move in column 3
    Then I receive a status code 409
    And I receive an error message "It is not your turn to play"

  Scenario: Play a move in a full column
    Given I am logged in with a valid JWT token
    And I am in a game with ID 1 in "InProgress" status
    And column 3 is already full
    When I send a request to play a move in column 3
    Then I receive a status code 400
    And I receive an error message "Column is full"
