using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Nameless.RabbitMQ.Course.Exercises {
    public sealed class SimpleQueueExercise : ExerciseBase {
        #region Public Override Properties

        public override string Code => "001";
        public override string Description => "Simple Queue Sample";

        #endregion

        #region Public Constructors

        public SimpleQueueExercise(IModel channel)
            : base(channel) { }

        #endregion

        #region Public Override Methods

        public override Task RunAsync(TextWriter output, CancellationToken cancellationToken) {
            const string queueName = "q.simple.queue";

            output.WriteLine("Configuring queue...");
            output.WriteLine("\t-Name: {0}", queueName);
            output.WriteLine("\t-Durable: false");
            output.WriteLine("\t-Exclusive: false");
            output.WriteLine("\t-AutoDelete: true");
            output.WriteLine("");

            Channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: true
            );

            output.WriteLine("Set queue listener...");
            output.WriteLine("\t-AutoAck: true");
            output.WriteLine("");
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (sender, args) => {
                output.WriteLine("Message received...");
                output.WriteLine("Content: {0}", MessageHelper.GetString(args.Body));
            };

            Channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer
            );

            output.WriteLine("Sending message...");
            Channel.BasicPublish(
                exchange: string.Empty,
                routingKey: queueName,
                body: MessageHelper.GetBytes(new {
                    Message = "Hello world!"
                })
            );

            return Task.CompletedTask;
        }

        #endregion
    }
}
