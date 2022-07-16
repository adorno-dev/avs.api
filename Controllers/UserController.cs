using AVS.API.Repositories;
using AVS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AVS.API.Controllers
{
    public class UserOutput
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
    }

    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TokenService tokenService;

        private readonly UserRepository userRepository;

        public UserController(UserRepository userRepository, TokenService tokenService)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
        }

        /// <summary>
        /// Get all users (debug)
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = userRepository.GetAll()?.Select(s => 
                new UserOutput { 
                    Id = s["Id"].AsString,
                    Username = s["Username"].AsString 
                }).ToList();

            await Task.CompletedTask;

            return Ok(users);
        }

        /// <summary>
        /// Get all contacts (debug / from your id)
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/contacts")]
        public async Task<IActionResult> GetContacts()
        {
            var ownerId = tokenService.GetUserIdFromRequest(HttpContext);

            var users = userRepository.GetContacts(ownerId)?.Select(s => 
                new UserOutput { 
                    Id = s["Id"].AsString,
                    Username = s["Username"].AsString 
                }).ToList();

            await Task.CompletedTask;

            return Ok(users);
        }
    }
}