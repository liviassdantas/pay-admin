using Confluent.Kafka;
using Newtonsoft.Json;
using pay_admin.Interfaces;
using System.Text.Json.Serialization;

namespace pay_admin.Service
{
    public class KafkaProducerService : IMessagesService
    {
        private readonly IProducer<string, string> _producer;
        public KafkaProducerService() { }
        public KafkaProducerService(string bootstrapServers)
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }
        public async Task ProduceMessageAsync<T>(string topic, T value)
        {
            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonConvert.SerializeObject(value)
            };

        await _producer.ProduceAsync(topic, message);
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
