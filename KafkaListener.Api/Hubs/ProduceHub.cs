using Microsoft.AspNetCore.SignalR;

namespace KafkaListener.Api.Hubs
{
    public class ProduceHub : Hub
    {
        public async Task SendProduceMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
