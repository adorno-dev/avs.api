using AVS.API.Models;
using AVS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AVS.API.Controllers
{
    [Authorize]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private TokenService tokenService;

        public ChatController(TokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [HttpGet("api/chats")]
        public async Task<IActionResult> IndexAsync()
        {
            await Task.CompletedTask;

            return Ok(tokenService.GetUserIdFromRequest(HttpContext));
        }

        [HttpPost("api/message")]
        public async Task<IActionResult> MessageAsync()
        {
            var chat = new Chat();
            
            await Task.CompletedTask;

            return Ok();
        }
    }
}