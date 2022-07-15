using System.ComponentModel.DataAnnotations;

namespace AVS.API.Contracts.Chat.Requests
{
    public class MessageRequest
    {
        public MessageRequest(string to, string body)
        {
            To = to;
            Body = body;
        }

        [Required]
        public string To { get; set; }

        [Required]
        public string Body { get; set; }
    }
}