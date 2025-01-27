# Acceptance Tests

Below are the high-level acceptance tests divided into three sections:

1. **User**  
2. **Game List**  
3. **Game**

Each feature includes one or more scenarios describing the required steps (given/when/then) and the expected outcomes.



## 1. User

### Feature: User Login

```Gherkin
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

```


### Feature: Logout

```Gherkin
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
```

<br/>

## 2. Game List

### Feature: View game history

```Gherkin
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
```

### Feature: View waiting games

```Gherkin
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
```

<br/>

## 3. Game

### Feature: Create a new game

```Gherkin
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

```

### Feature: Join an existing game

```Gherkin
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
```


### Feature: Leave a game

```Gherkin
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
```

### Feature: Play a move in a game

```Gherkin	
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
```

### Conclusion
These features and scenarios form the basis of our acceptance tests for the Connect4-like system. They define the expected behavior of user authentication, game listing, and game interaction, providing clear pass/fail criteria to validate the application’s functionality.
