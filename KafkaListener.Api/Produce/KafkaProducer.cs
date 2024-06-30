using Confluent.Kafka;
using KafkaListener.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;

namespace KafkaListener.Api.Produce
{
    public class KafkaProducer : IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly IHubContext<ProduceHub, IProduceHub> _hubContext;

        public KafkaProducer(IHubContext<ProduceHub, IProduceHub> hubContext, string bootstrapServers)
        {
            _hubContext = hubContext;

            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            _producer = new ProducerBuilder<string, string>(config)
           .SetErrorHandler((_, error) => OnError(_, error))
           .Build();
        }
        private void OnError(IProducer<string, string> producer, Error error)
        {
            _hubContext.Clients.All.ReceiveMessage($"NOT - Mesaj kuyruğa iletilemedi. Bağlantı Problemi Yaşanıyor.");
            Console.WriteLine($"Kafka hatası: {error.Reason}");
        }
        public async Task ProduceAndSendToClientsAsync(string topic, string key, string value)
        {

            try
            {
                var message = new Message<string, string>
                {
                    Key = key,
                    Value = value
                };

                DeliveryResult<string, string> result = await _producer.ProduceAsync(topic, message);
                if (result.Status == PersistenceStatus.Persisted)
                {
                    await _hubContext.Clients.All.ReceiveMessage($"OK - Mesaj Kuyruğa Gönderildi {DateTime.Now}: {value}");
                }
                else
                {
                    await _hubContext.Clients.All.ReceiveMessage($"ERR - Mesaj Gönderilemedi {DateTime.Now}: {value}");
                }
                
            }
            catch (ProduceException<string, string> e)
            {
                await _hubContext.Clients.All.ReceiveMessage($"ERR - Mesaj Gönderilemedi {DateTime.Now}: {value}");
                Console.WriteLine($"Hata: {e.Error.Reason}");
            }
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(1)); // Mevcut mesajların gönderilmesi için bekleme süresi
            _producer.Dispose(); // Producer'ı serbest bırakma
        }
    }
}
