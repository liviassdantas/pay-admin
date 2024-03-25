using Confluent.Kafka;
using Newtonsoft.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace pay_admin.Service
{
    public abstract class KafkaConsumerService<T> : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        public KafkaConsumerService(string bootstrapServers, string groupId, string topic)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe(topic); 
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested) 
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);
                    if(result != null && !result.IsPartitionEOF)
                    {
                        var value = JsonConvert.DeserializeObject<T>(result.Value);
                        await ConsumeAsync(value, stoppingToken);
                    }
                }catch (OperationCanceledException)
                {
                    throw;
                }catch (Exception ex)
                {
                    throw new Exception(ex.ToString(), ex);
                }
            }
            _consumer.Close();
        }
        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }
        protected abstract Task ConsumeAsync(T value, CancellationToken stoppingToken);


    }
}
