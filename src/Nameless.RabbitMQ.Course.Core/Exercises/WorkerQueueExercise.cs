using RabbitMQ.Client;

namespace Nameless.RabbitMQ.Course.Exercises {
    public sealed class WorkerQueueExercise : ExerciseBase {
        #region Public Override Properties

        public override int Code => 2;
        public override string Description => "Worker Queue Sample";

        #endregion

        #region Public Constructors

        public WorkerQueueExercise(IModel channel)
            : base(channel) { }

        #endregion

        #region Public Override Methods

        public override Task RunAsync(TextWriter output, CancellationToken cancellationToken) {

            const string exchangeName = "ex.worker.exchange";
            DeclareExchange(
                exchangeName: exchangeName,
                ExchangeType.Direct,
                output: output
            );

            const string queueName = "q.worker.queue";
            DeclareQueue(
                exchangeName: exchangeName,
                queueName: queueName,
                output: output
            );

            DeclareConsumer(
                queueName: queueName,
                handler: message => {
                    output.WriteLine("Hi! I'm consumer A");
                    output.WriteLine($"Received message: {message}");
                    output.WriteLine();
                },
                output: output
            );

            DeclareConsumer(
                queueName: queueName,
                handler: message => {
                    output.WriteLine("Hi! I'm consumer B");
                    output.WriteLine($"Received message: {message}");
                    output.WriteLine();
                },
                output: output
            );

            const int totalMessages = 10;
            var messages = new List<Message>();
            for (var idx = 0; idx < totalMessages; idx++) {
                messages.Add(new(
                    ExchangeName: exchangeName,
                    Payload: new {
                        Message = $"{idx + 1}: Hello world!"
                    })
                );
            }

            PublishMessageBatch(
                messages: [.. messages],
                output: output
            );

            return Task.CompletedTask;
        }

        #endregion
    }
}
