### This project has been copied from [Marc Proux's Connect4](https://gitlab.arcadia-hub.com/cours/n-tiers/Connect-4). 

<h2 align="center">
  Connect 4<br/>
  <br/>
  <img alt="Logo" src="https://github.com/user-attachments/assets/6c942d0f-7568-4d76-bf9d-166a13dacaa9" width="200" height="200"/>
</h2>

[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/built-with-love.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/powered-by-coffee.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/license-mit.svg)](https://forthebadge.com)

Third-year Computer Science and Network Engineering project at ENSISA as part of the N-Tiers module.


## Requirements
### Database (Connect4.Infrastructure)

Database: SQLite (Required)

[SQLite Download](https://www.sqlite.org/download.html)

[DB Browser for SQLite](https://sqlitebrowser.org/dl/)

## Run the project
### Database (Connect4.Infrastructure)
Go to the Connect4.Infrastructure folder and run the following commands:

```bash
  dotnet ef migrations add NameOfMigration --context DatabaseContext
  dotnet ef database update --context DatabaseContext
```

### API (Connect4.API) and UI (Connect4.UI)
Run the script in the root folder :

Windows:
```bash
  ./run.bat (not always working on first try)
```

MacOS/Linux:
```bash
  ./run.sh
```

### Access the application
Go to the following URL in your browser:

```bash
  https://localhost:7213
```

You can connect with one of the following credentials:

| Username | Password |
|----------|----------|
| Marc     | marc     |
| Geryes   | geryes   |
| Matthias | matthias |
| Alice    | alice    |
| Bob      | bob      |
| John     | john     |
| Gabin    | gabin    |

⚠️ John and Gabin are already in a game. With those accounts, you can't access the game list and game history unless leaving th egame (or complete it) ⚠️

## Testing

### Unit tests
Multiple unit tests have been written for the Connect4 project. To run them, go to the project folder and run the following command:

```bash
  dotnet test Connect-4.sln --filter Category=Unit
```

### Acceptance tests
Multiple acceptance tests have been written for the Connect4 project. To run them, go to the project folder and run the following command:

```bash
  dotnet test Connect-4.sln --filter Category=Acceptance
```

For more information, see the [Acceptance Tests](doxygen/AT.md) documentation.


## Domain Model

You can find the domain model and database schema diagrams in the [Domain Model](DomainModel.md) documentation.

## Documentation

You can find the project documentation in the `/documentation/html/index.html` file.

## License

Distributed under the MIT License. See [LICENSE.md](LICENSE.md) for more information.


<footer>
<p align="center">
Made with ❤️ by Geryes, Matthias and Marc, students at the <a href="https://www.ensisa.uha.fr">ENSISA</a>.
</p>
</footer>
