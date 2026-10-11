using Anthropic;
using Anthropic.Models.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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
            if (request.Messages is null || request.Messages.Count == 0)
                return BadRequest("No messages sent.");

            var email = User.FindFirst("email")?.Value ?? "";

            var answer = await agent.AskAsync(request.Messages, email);
            return Ok(answer);
        }
    }
}
public record AskRequest(List<NWCodeFirstMVC.Api.Agents.ChatTurn> Messages);
