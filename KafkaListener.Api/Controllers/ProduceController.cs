using KafkaListener.Api.Entities;
using KafkaListener.Api.Hubs;
using KafkaListener.Api.Produce;
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
        private readonly KafkaProducer _producer;
        public ProduceController(IHubContext<ProduceHub, IProduceHub> hubContext, KafkaProducer producer)
        {
            _producer = producer;
            _hubContext = hubContext;
        }

        [HttpPost("sendProduce")]
        public async Task<IActionResult> PostMessage([FromBody] Messages message)
        {
            await _producer.ProduceAndSendToClientsAsync("example-topic", "example-key", message.KafkaJsonMessage);     
            return Ok(new { Message = "Message sent." });
        }
    }
}
