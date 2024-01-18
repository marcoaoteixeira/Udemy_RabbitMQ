using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Nameless.RabbitMQ.Course.Exercises {
    public sealed class WorkerQueueExercise : ExerciseBase {
        #region Public Override Properties

        public override string Code => "002";
        public override string Description => "Worker Queue Sample";

        #endregion

        #region Public Constructors

        public WorkerQueueExercise(IModel channel)
            : base(channel) { }

        #endregion

        #region Public Override Methods

        public override Task RunAsync(TextWriter output, CancellationToken cancellationToken) {
            const string queueName = "q.worker.queue";

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

            var autoAck = true;

            var consumerA = CreateConsumer(
                writer: output,
                identifier: "A",
                autoAck: autoAck
            );
            Channel.BasicConsume(
                queue: queueName,
                autoAck: autoAck,
                consumerA
            );

            var consumerB = CreateConsumer(
                writer: output,
                identifier: "B",
                autoAck: autoAck
            );
            Channel.BasicConsume(
                queue: queueName,
                autoAck: autoAck,
                consumerB
            );

            var total = 10;
            output.WriteLine("Sending {0} messages...", total);
            var batch = Channel.CreateBasicPublishBatch();
            for (var idx = 0; idx < total; idx++) {
                batch.Add(
                    exchange: string.Empty,
                    routingKey: queueName,
                    mandatory: false,
                    properties: Channel.CreateBasicProperties(),
                    body: MessageHelper.GetBytes(new {
                        Message = $"{idx + 1}: Hello world!"
                    })
                );
            }
            batch.Publish();

            return Task.CompletedTask;
        }

        private EventingBasicConsumer CreateConsumer(TextWriter writer, string identifier, bool autoAck) {
            writer.WriteLine("Set queue listener {0}...", identifier);
            writer.WriteLine("\t-AutoAck: {0}", autoAck);
            writer.WriteLine("");
            var result = new EventingBasicConsumer(Channel);
            result.Received += (sender, args) => {
                writer.WriteLine("I'm consumer {0}", identifier);
                writer.WriteLine("Message: {0}", MessageHelper.GetString(args.Body));
            };
            return result;
        }

        #endregion
    }
}
