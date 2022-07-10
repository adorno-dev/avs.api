using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using avs.api.Contracts.Authentication.Responses;
using AVS.API.Contracts.Authentication.Requests;
using AVS.API.Contracts.Authentication.Responses;
using AVS.API.Models;
using AVS.API.Repositories;
using AVS.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AVS.API.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region +Private Fields

        private UserRepository userRepository;
        private UserRefreshTokenRepository userRefreshTokenRepository;
        private TokenService tokenService;

        #endregion

        #region +Constructors

        public AuthenticationController(UserRepository userRepository, UserRefreshTokenRepository userRefreshTokenRepository, TokenService tokenService)
        {
            this.userRepository = userRepository;
            this.userRefreshTokenRepository = userRefreshTokenRepository;
            this.tokenService = tokenService;
        }

        #endregion
    
        #region +Endpoints

        [HttpPost("api/sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            userRepository.Get(request.Email, out User? user);

            if (user is not null) return BadRequest(SignInResponse.EmailAlreadyExists());

            if (!request.Password.Equals(request.ConfirmPassword)) return BadRequest(SignUpResponse.MismatchOnPasswords());

            user = new User(request.Username, request.Email, request.Password);

            userRepository.Create(user);

            await Task.CompletedTask;

            return NoContent();
        }

        [HttpPost("api/sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            userRepository.Get(request.Email, out User? user);

            if (user == null) 
                return Unauthorized(SignInResponse.NotAuthorized());

            if (!user.VerifyPassword(request.Password)) 
                return Unauthorized(SignInResponse.NotAuthorized());
            
            tokenService.GenerateToken(user, out string token);
            tokenService.GenerateRefreshToken(out string refreshToken);

            userRefreshTokenRepository.Save(user.Id, refreshToken);

            await Task.CompletedTask;

            return Ok(SignInResponse.Ok(token, refreshToken));
        }

        [HttpPost("api/refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            string userId;

            long unixTokenExpiration, unixUtcNow;

            tokenService.GetClaimsPrincipalFromExpiredToken(request.Token, out ClaimsPrincipal claimsPrincipal);

            userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            userRefreshTokenRepository.Get(userId, out string? storedRefreshToken);

            if (storedRefreshToken == null || storedRefreshToken != request.RefreshToken) 
                return BadRequest(RefreshTokenResponse.InvalidRefreshToken());

            unixUtcNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            unixTokenExpiration = long.Parse(claimsPrincipal.Claims.First(x => x.Type.Equals(JwtRegisteredClaimNames.Exp)).Value);

            if (unixTokenExpiration > unixUtcNow) 
                return BadRequest(RefreshTokenResponse.TokenNotExpired());

            tokenService.GenerateToken(claimsPrincipal.Claims, out string newToken);
            tokenService.GenerateRefreshToken(out string newRefreshToken);

            userRefreshTokenRepository.Delete(userId, request.RefreshToken);
            userRefreshTokenRepository.Save(userId, newRefreshToken);

            await Task.CompletedTask;

            return Ok(RefreshTokenResponse.Ok(newToken, newRefreshToken));
        }

        #endregion
    }
}