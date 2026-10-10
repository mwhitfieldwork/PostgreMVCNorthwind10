using Anthropic;
using Anthropic.Models.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NWCodeFirstMVC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AgentController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("hello")]
        public async Task<IActionResult> Hello()
        {
            var client = new AnthropicClient { ApiKey = _config["Anthropic:ApiKey"] };

            var message = await client.Messages.Create(new MessageCreateParams
            {
                Model = "claude-haiku-4-5",
                MaxTokens = 200,
                Messages =
                [
                    new() { Role = Role.User, Content = "Say hello to Mike in one short sentence." }
                ]
            });

            var reply = "";
            foreach (var block in message.Content)
            {
                if (block.TryPickText(out var textBlock))
                    reply += textBlock.Text;
            }

            return Ok(reply);
        }
        [Authorize]
        [HttpPost("ask")]
        public async Task<IActionResult> Ask(
        [FromBody] AskRequest request,
        [FromServices] NWCodeFirstMVC.Api.Agents.RestockAgent agent)
        {
            var email = User.FindFirst("email")?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("No email found in your login token.");

            var answer = await agent.AskAsync(request.Question, email);
            return Ok(answer);
        }
    }
}
public record AskRequest(string Question);
