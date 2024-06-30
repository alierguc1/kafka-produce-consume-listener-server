namespace KafkaListener.Api.Hubs
{
    public interface IProduceHub
    {
        public Task ReceiveMessage(string sendJsonMessage);

    }
}
