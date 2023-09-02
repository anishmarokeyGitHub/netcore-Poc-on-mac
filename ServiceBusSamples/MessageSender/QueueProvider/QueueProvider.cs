using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using MessageSender.QueueProvider.Abstractions;
using Microsoft.Extensions.Options;
namespace MessageSender.QueueProvider;

internal sealed class QueueProvider : IQueueProvider
{
    #region Variables
    private readonly IQueueBuilder _queueBuilder;
    private readonly IOptions<QueueOptions> _options;
    #endregion

    #region  Constructor
    public QueueProvider(IQueueBuilder queueBuilder, IOptions<QueueOptions> options)
    {
        _queueBuilder = queueBuilder;
        _options = options;
    }
    #endregion

    #region  IQueueProvider 
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception> <summary>
    public async Task<T> ReceiveFromQueueAsync<T>()
    {
        var queueClient = _queueBuilder.Build();
        var receiver = queueClient.CreateReceiver(_options.Value.QueueName);
        var receivedMessage = await receiver.ReceiveMessageAsync();

        return ConvertFromServiceBusMessage<T>(receivedMessage);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <exception cref="NotImplementedException"></exception> <summary>

    public async Task PushToQueueAsync<T>(T message)
    {
        var queueClient = _queueBuilder.Build();
        var sender = queueClient.CreateSender(_options.Value.QueueName);
        await sender.SendMessageAsync(ConvertToServiceBusMessage(message));

    }
    #endregion

    private ServiceBusMessage ConvertToServiceBusMessage<T>(T message)
    {
        // Serialize the generic message to JSON and create a ServiceBusMessage
        string jsonMessage = JsonSerializer.Serialize(message);
        return new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage));
    }

    private T ConvertFromServiceBusMessage<T>(ServiceBusReceivedMessage receivedMessage)
    {
        // Deserialize the JSON message from the received ServiceBusMessage
        string jsonMessage = Encoding.UTF8.GetString(receivedMessage.Body);
        return JsonSerializer.Deserialize<T>(jsonMessage);
    }
}
