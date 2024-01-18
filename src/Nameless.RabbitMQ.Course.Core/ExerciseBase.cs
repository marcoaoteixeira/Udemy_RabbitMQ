using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Nameless.RabbitMQ.Course {
    public abstract class ExerciseBase {
        #region Public Abstract Properties

        public abstract int Code { get; }
        public abstract string Description { get; }

        #endregion

        #region Protected Properties

        protected IModel Channel { get; }

        #endregion

        #region Protected Constructors

        protected ExerciseBase(IModel channel) {
            Channel = channel ?? throw new ArgumentNullException(nameof(channel));
        }

        #endregion

        #region Protected Virtual Methods

        protected virtual void DeclareExchange(string exchangeName, ExchangeType type, bool durable = false, bool autoDelete = true, TextWriter? output = null) {
            var inner = output ?? TextWriter.Null;

            inner.WriteLine($"Declaring exchange...");
            inner.WriteLine($"  - Name: {exchangeName}");
            inner.WriteLine($"  - Type: {type}");
            inner.WriteLine($"  - Durable: {durable}");
            inner.WriteLine($"  - AutoDelete: {autoDelete}");
            inner.WriteLine();
            Channel.ExchangeDeclare(
                exchange: exchangeName,
                type: type.GetDescription(),
                durable: durable,
                autoDelete: autoDelete
            );
        }

        protected virtual void DeclareQueue(string exchangeName, string queueName, string? routingKey = null, bool durable = false, bool autoDelete = true, TextWriter? output = null) {
            var inner = output ?? TextWriter.Null;

            inner.WriteLine($"Declaring queue...");
            inner.WriteLine($"  - Exchange: {exchangeName}");
            inner.WriteLine($"  - Name: {queueName}");
            inner.WriteLine($"  - RoutingKey: {routingKey}");
            inner.WriteLine($"  - Durable: {durable}");
            inner.WriteLine($"  - AutoDelete: {autoDelete}");
            inner.WriteLine();
            Channel.QueueDeclare(
                queue: queueName,
                durable: durable,
                exclusive: false,
                autoDelete: autoDelete
            );

            Channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: routingKey ?? string.Empty
            );
        }

        protected virtual void DeclareConsumer(string queueName, Action<string> handler, bool autoAck = true, TextWriter? output = null) {
            var inner = output ?? TextWriter.Null;

            inner.WriteLine("Declaring consumer...");
            inner.WriteLine($"  - Queue: {queueName}");
            inner.WriteLine($"  - AutoAck: {autoAck}");
            inner.WriteLine();

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (sender, args)
                => handler(PayloadHelper.GetString(args.Body));

            Channel.BasicConsume(
                queue: queueName,
                autoAck: autoAck,
                consumer
            );
        }

        protected virtual void PublishMessage(Message message, TextWriter? output = null) {
            var inner = output ?? TextWriter.Null;

            inner.WriteLine("Publishing message...");
            inner.WriteLine();
            Channel.BasicPublish(
                exchange: message.ExchangeName,
                routingKey: message.RoutingKey ?? string.Empty,
                body: PayloadHelper.GetBytes(message.Payload ?? new { })
            );
        }

        protected virtual void PublishMessageBatch(Message[] messages, TextWriter? output = null) {
            var inner = output ?? TextWriter.Null;

            inner.WriteLine($"Publishing messages (total: {messages.Length})...");
            inner.WriteLine();
            var batch = Channel.CreateBasicPublishBatch();
            foreach (var message in messages) {
                batch.Add(
                    message.ExchangeName,
                    message.RoutingKey ?? string.Empty,
                    mandatory: false,
                    Channel.CreateBasicProperties(),
                    PayloadHelper.GetBytes(message.Payload ?? new { })
                );
            }
            batch.Publish();
        }

        #endregion

        #region Public Abstract Methods

        public abstract Task RunAsync(TextWriter output, CancellationToken cancellationToken);

        #endregion
    }
}
