namespace pay_admin.Interfaces
{
    public interface IMessagesService : IDisposable
    {
        Task ProduceMessageAsync<T>(string topic, T message);
    }
}
