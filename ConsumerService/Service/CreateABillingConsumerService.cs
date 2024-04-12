

namespace ConsumerService.Service
{
    public class CreateABillingConsumerService : KafkaConsumerService
    {
        private readonly string _topic;
        private readonly string _groupId;

        public CreateABillingConsumerService(IConfiguration configuration, ILoggerFactory loggerFactory, string groupId, string topic) : base(configuration, loggerFactory, groupId, topic)
        {
            _topic = topic;
            _groupId = groupId;
        }
    }
}
