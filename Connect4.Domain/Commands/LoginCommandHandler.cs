using MediatR;
using Connect4.Domain.Repositories;
using Connect4.Domain.Services;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Connect4.Common.Model;
using Connect4.Domain.Exceptions;

namespace Connect4.Domain.Commands
{
    /// <summary>
    /// Handles the <see cref="LoginCommand"/>, which attempts to authenticate a user
    /// and generate a JWT token upon successful login.
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly UserQueryRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">Repository to query users from the data store.</param>
        /// <param name="passwordService">Service to verify the provided password against its hashed counterpart.</param>
        /// <param name="configuration">Used to retrieve JWT settings such as secret key, issuer, and audience.</param>
        public LoginCommandHandler(
            UserQueryRepository userRepository,
            IPasswordService passwordService,
            IConfiguration configuration
        )
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        /// <summary>
        /// Handles the login process: 
        /// <list type="number">
        /// <item>Retrieves the user from the data store.</item>
        /// <item>Verifies the password.</item>
        /// <item>Generates the JWT token.</item>
        /// </list>
        /// </summary>
        /// <param name="request">The <see cref="LoginCommand"/> containing the username and password.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>The generated JWT token.</returns>
        /// <exception cref="UnauthorizedDomainException">Thrown if the user does not exist or if the password is invalid.</exception>
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1) Retrieve the user from the repository.
            var user = await _userRepository.GetUserByUsername(request.Username);
            if (user == null)
            {
                // User not found => 401
                throw new UnauthorizedDomainException("Invalid username or password.");
            }

            // 2) Validate the provided password using the password service.
            bool isValidPassword = _passwordService.VerifyPassword(
                request.Password,
                user.HashPwd
            // If a salt is stored separately, retrieve user.Salt and adjust accordingly.
            );

            if (!isValidPassword)
            {
                // Password mismatch => 401
                throw new UnauthorizedDomainException("Invalid username or password.");
            }

            // 3) If everything is valid, generate and return the JWT.
            var token = GenerateJwtToken(user);
            return token;
        }

        /// <summary>
        /// Generates a JWT token for the authenticated user.
        /// </summary>
        /// <param name="user">The user for whom the token is being generated.</param>
        /// <returns>A string representation of the JWT token.</returns>
        private string GenerateJwtToken(User user)
        {
            // Retrieve the secret key from configuration
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Create signing credentials using HMAC-SHA256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Build the list of claims. 
            // Include user ID and username for identification.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            // Generate a unique token identifier (JTI) 
            // to support token revocation (blacklist) if needed.
            var jti = Guid.NewGuid().ToString();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));

            // Construct the JWT token with expiry time and signing credentials.
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6), // Token validity duration
                signingCredentials: creds
            );

            // Return the token as a serialized string.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
