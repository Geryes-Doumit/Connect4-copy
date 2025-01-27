using System;

namespace Connect4.Common.Model;

public class BoardDto
{
    public int Id { get; set; }

    public string CurrentPlayer { get; set; }

    public string State { get; set; }
}
