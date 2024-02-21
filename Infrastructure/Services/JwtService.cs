using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

// Service for handling JWT token generation and validation
public class JwtService(IConfiguration configuration, UserManager<User> userManager) : IJwtService
{
    // Configuration section for JWT settings
    private readonly IConfigurationSection _jwtSection = configuration.GetSection("Jwt");

    // Get the signing credentials for JWT
    public SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSection["Key"]!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    // Get the claims for a user to be included in the JWT token
    public async Task<List<Claim>> GetClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            // Add email and userId claims
            new("email", user.Email!),
            new("fullName", $"{user.FirstName} {user.LastName}"),
            new("userId", user.Id.ToString().ToUpper())
        };

        // Retrieve user roles and add them as claims
        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return claims;
    }

    // Generate JWT token options based on provided signing credentials and claims
    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            _jwtSection["Issuer"],
            _jwtSection["Audience"],
            claims,
            expires: DateTime.Now.AddDays(Convert.ToInt32(_jwtSection["DurationInDays"])),
            signingCredentials: signingCredentials
        );
        return tokenOptions;
    }
}