namespace Connect4.Common.Model;

/// <summary>
/// Data Transfer Object for login responses.
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Gets or sets the success message of the login operation.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the JWT token returned upon successful login.
    /// </summary>
    public string Token { get; set; }
}
