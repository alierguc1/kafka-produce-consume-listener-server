using KafkaListener.Api.Entities;
using KafkaListener.Api.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KafkaListener.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduceController : ControllerBase
    {
        private readonly IHubContext<ProduceHub,IProduceHub> _hubContext;

        public ProduceController(IHubContext<ProduceHub, IProduceHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("sendProduce")]
        public async Task<IActionResult> PostMessage([FromBody] Messages message)
        {
            await _hubContext.Clients.All.ReceiveMessage(message.KafkaJsonMessage);
            return Ok(new { Message = "Message sent." });
        }
    }
}
