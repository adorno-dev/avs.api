using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AVS.API.Models;
using AVS.API.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace AVS.API.Services
{
    public class TokenService
    {
        #region +Private Methods

        private string GenerateToken(ClaimsIdentity claimsIdentity)
        {
            var handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor()
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(TokenSettings.GetSecret()),
                SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddMinutes(120),
                Subject = claimsIdentity
            };

            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        #endregion

        #region +Public Methods

        public string GenerateToken(User user) => GenerateToken(
            new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id),  
                new Claim(ClaimTypes.Email, user.Email)
            })
        );

        public string GenerateToken(IEnumerable<Claim> claims) => GenerateToken(new ClaimsIdentity(claims));

        public string GenerateRefreshToken()
        {
            using var generator = RandomNumberGenerator.Create();

            var random = new byte[32];

            generator.GetBytes(random);

            return Convert.ToBase64String(random);
        }

        public ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var parameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(TokenSettings.GetSecret()),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var claimsPrincipal = handler.ValidateToken(token, parameters, out var securityToken);

            if (securityToken is not JwtSecurityToken securityJwtToken || !
                securityJwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token.");
            
            return claimsPrincipal;
        }

        public string GetUserIdFromRequest(HttpContext context)
        {
            var token = context.Request.Headers.Authorization.First().Split(" ")[1];

            var claims = GetClaimsPrincipalFromExpiredToken(token);

            return claims.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        #endregion
    
        #region +Extension Methods

        public void GenerateToken(User user, out string token) => token = GenerateToken(user);

        public void GenerateToken(IEnumerable<Claim> claims, out string token) => token = GenerateToken(claims);

        public void GenerateRefreshToken(out string refreshToken) => refreshToken = GenerateRefreshToken();

        public void GetClaimsPrincipalFromExpiredToken(string token, out ClaimsPrincipal claimsPrincipal) 
            => claimsPrincipal = GetClaimsPrincipalFromExpiredToken(token);

        #endregion
    }
}