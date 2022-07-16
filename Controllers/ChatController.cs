using AVS.API.Contracts.Chat.Requests;
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
        private ChatService chatService;

        public ChatController(TokenService tokenService, ChatService chatService)
        {
            this.tokenService = tokenService;
            this.chatService = chatService;
        }

        /// <summary>
        /// Get all chats (debug)
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/chats")]
        public async Task<IActionResult> GetChatsAsync() => Ok(await chatService.GetChatsAsync());

        /// <summary>
        /// Get an especific chat
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/chats/{id}")]
        public async Task<IActionResult> GetChatAsync(string id) => Ok(await chatService.GetChatAsync(id));

        /// <summary>
        /// Gets a chat from your contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/chats/contact/{id}")]
        public async Task<IActionResult>GetChatByContactAsync(string id)
        {
            var userId = tokenService.GetUserIdFromRequest(HttpContext);
            var chatId = chatService.GetChatId(new [] {userId, id});
            return Ok(await chatService.GetChatAsync(chatId));
        }

        /// <summary>
        /// Send message to an especific contact
        /// </summary>
        /// <param name="messageRequest"></param>
        /// <returns>Returns generated chat ID</returns>
        [HttpPost("api/message")]
        public async Task<IActionResult> MessageAsync([FromBody] MessageRequest messageRequest)
        {
            var senderId = tokenService.GetUserIdFromRequest(HttpContext);
            var toId = messageRequest.To;
            var users = new string[] {senderId, toId};
            var message = new ChatMessage(senderId, messageRequest.Body);

            var chatId = chatService.GetChatId(users);

            if (string.IsNullOrEmpty(chatId))
                chatId = chatService.CreateChat(senderId, users, new ChatMessage(senderId, messageRequest.Body));
            else
                chatService.CreateChatMessage(chatId, message);

            await Task.CompletedTask;

            return Ok(chatId);
        }
    }
}