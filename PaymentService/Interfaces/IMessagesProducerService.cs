namespace pay_admin.Interfaces
{
    public interface IMessagesProducerService : IDisposable
    {
        Task ProduceMessageAsync<T>(string topic, T message);
    }
}
