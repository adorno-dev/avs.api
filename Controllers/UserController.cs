using AVS.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AVS.API.Controllers
{
    public class UserOutput
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
    }

    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository userRepository;

        public UserController(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

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
    }
}