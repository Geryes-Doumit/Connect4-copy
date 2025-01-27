using System;
using Xunit;
using Connect4.Common.Model;
using Connect4.Domain.Services;

namespace Connect4.Domain.Services.UnitTests;

/// <summary>
/// Unit tests for the <see cref="GameService"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class GameServiceTest
{
    private readonly IGameService _gameService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameServiceTest"/> class.
    /// </summary>
    public GameServiceTest()
    {
        _gameService = new GameService();
    }

    /// <summary>
    /// Tests the <see cref="GameService.ValidateMove"/> method with invalid column values.
    /// </summary>
    [Fact]
    public void ValidateMove_InvalidColumn_ReturnsFalse()
    {
        // Arrange
        var board = new Board("testUser", "0000000;0000000;0000000;0000000;0000000;0000000");

        // Act
        var result = _gameService.ValidateMove(board, -1);

        // Assert
        Assert.False(result);

        // Act
        result = _gameService.ValidateMove(board, 7);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="GameService.ValidateMove"/> method with a valid column value.
    /// </summary>
    [Fact]
    public void ValidateMove_ValidColumn_ReturnsTrue()
    {
        // Arrange
        var board = new Board("testUser", "0000000;0000000;0000000;0000000;0000000;0000000");

        // Act
        var result = _gameService.ValidateMove(board, 3);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="GameService.ValidateMove"/> method with a full column.
    /// </summary>
    [Fact]
    public void ValidateMove_ColumnFull_ReturnsFalse()
    {
        // Arrange
        var board = new Board("testUser", "1000000;1000000;1000000;1000000;1000000;1000000");

        // Act
        var result = _gameService.ValidateMove(board, 0);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="GameService.UpdateBoard"/> method with a valid move.
    /// </summary>
    [Fact]
    public void UpdateBoard_ValidMove_UpdatesBoard()
    {
        // Arrange
        var board = new Board("testUser", "0000000;0000000;0000000;0000000;0000000;0000000");
        var userColor = '1';
        var column = 3;

        // Act
        var updatedBoard = _gameService.UpdateBoard(board, userColor, column);

        // Assert
        Assert.Equal("0000000;0000000;0000000;0000000;0000000;0001000", updatedBoard);
    }

    /// <summary>
    /// Tests the <see cref="GameService.CheckWin"/> method with a horizontal win.
    /// 
    [Fact]
    public void CheckWin_HorizontalWin_ReturnsTrue()
    {
        // Arrange
        var board = new Board("testUser", "0000000;0000000;0000000;0000000;0000000;1111000");
        var userColor = '1';

        // Act
        var result = _gameService.CheckWin(board, userColor);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="GameService.CheckWin"/> method with a vertical win.
    /// </summary>
    [Fact]
    public void CheckWin_VerticalWin_ReturnsTrue()
    {
        // Arrange
        var board = new Board("testUser", "0000000;0000000;1000000;1000000;1000000;1000000");
        var userColor = '1';

        // Act
        var result = _gameService.CheckWin(board, userColor);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="GameService.CheckWin"/> method with a diagonal win.
    /// </summary>
    [Fact]
    public void CheckWin_DiagonalWin_ReturnsTrue()
    {
        // Arrange
        var board = new Board("testUser", "0000000;0000000;0001000;0010000;0100000;1000000");
        var userColor = '1';

        // Act
        var result = _gameService.CheckWin(board, userColor);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="GameService.CheckWin"/> method with no win.
    /// </summary>
    [Fact]
    public void CheckWin_NoWin_ReturnsFalse()
    {
        // Arrange
        var board = new Board("testUser", "0000000;0000000;0000000;0000000;0000000;0000000");
        var userColor = '1';

        // Act
        var result = _gameService.CheckWin(board, userColor);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="GameService.CheckDraw"/> method with a full board.
    /// </summary>
    [Fact]
    public void CheckDraw_BoardFull_ReturnsTrue()
    {
        // Arrange
        var board = new Board("testUser", "1111111;2222222;1111111;2222222;1111111;2222222");

        // Act
        var result = _gameService.CheckDraw(board);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="GameService.CheckDraw"/> method with a board that is not full.
    /// </summary>
    [Fact]
    public void CheckDraw_BoardNotFull_ReturnsFalse()
    {
        // Arrange
        var board = new Board("testUser", "0000000;2222222;1111111;2222222;1111111;1111111");

        // Act
        var result = _gameService.CheckDraw(board);

        // Assert
        Assert.False(result);
    }
}