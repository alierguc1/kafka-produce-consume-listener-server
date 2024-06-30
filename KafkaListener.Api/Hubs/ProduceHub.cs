using Microsoft.AspNetCore.SignalR;

namespace KafkaListener.Api.Hubs
{
    public class ProduceHub : Hub<IProduceHub>
    {
        public async Task SendMessage(string sendJsonMessage)
        {
            await Clients.All.ReceiveMessage(sendJsonMessage);
        }
    }
}
