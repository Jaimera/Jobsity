using RabbitMQ.Client;

namespace JobsityChallenge.CrossCutting.RabbitMQ
{
    public interface IMessageQueue
    {
        void QueueMessage(string message);
    }
}
