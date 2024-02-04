using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Interfaces
{
    // Interface for JWT (JSON Web Token) related operations
    public interface IJwtService
    {
        // Get the signing credentials used for token generation
        SigningCredentials GetSigningCredentials();

        // Get a list of claims associated with a user
        Task<List<Claim>> GetClaimsAsync(User user);

        // Generate JWT token options with specified signing credentials and claims
        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
    }
}
