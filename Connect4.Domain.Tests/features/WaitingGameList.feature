Feature: View waiting games
  As a user
  I want to view a list of games that are waiting for players
  So that I can choose a game to join

  Scenario: Successfully view waiting games
    Given I am logged in with a valid JWT token
    And there are games with the status "WaitingForPlayers"
    When I send a request to view the waiting games
    Then I receive a status code 200
    And I receive a list of games with their names, hosts, and IDs

  Scenario: View waiting games when no games are available
    Given I am logged in with a valid JWT token
    And there are no games with the status "WaitingForPlayers"
    When I send a request to view the waiting games
    Then I receive a status code 200
    And I receive an empty list

  Scenario: View waiting games while being busy in a game
    Given I am logged in with a valid JWT token
    And I am currently in a game with ID 1 in "InProgress" status
    When I send a request to view the waiting games
    Then I receive a status code 409
    And I receive an error message "You are busy in game 1, cannot access waiting games."
