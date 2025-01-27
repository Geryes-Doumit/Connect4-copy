using System;

namespace Connect4.UI.Data;

public class BoardUtilities
{
    public static char[,] GetBoardArrayFromString(string boardString)
    {
        char[][] boardDoubleArray = boardString.Split(";")
                                        .Select(row => row.ToCharArray())
                                        .ToArray();

        char[,] boardArray = new char[boardDoubleArray.Length, boardDoubleArray[0].Length];

        for (int i = 0; i < boardDoubleArray.Length; i++)
        {
            for (int j = 0; j < boardDoubleArray[i].Length; j++)
            {
                boardArray[i, j] = boardDoubleArray[i][j];
            }
        }

        return boardArray;
    }

    public static char[,] GetEmptyBoard() {
        return new char[6, 7];
    }
}
