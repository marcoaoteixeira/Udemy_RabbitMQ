using RabbitMQ.Client;

namespace Nameless.RabbitMQ.Course.Exercises {
    public sealed class SimpleQueueExercise : ExerciseBase {
        #region Public Override Properties

        public override int Code => 1;
        public override string Description => "Simple Queue Sample";

        #endregion

        #region Public Constructors

        public SimpleQueueExercise(IModel channel)
            : base(channel) { }

        #endregion

        #region Public Override Methods

        public override Task RunAsync(TextWriter output, CancellationToken cancellationToken) {
            const string exchangeName = "ex.simple.exchange";
            DeclareExchange(exchangeName, ExchangeType.Direct, output: output);

            const string queueName = "q.simple.queue";
            DeclareQueue(exchangeName, queueName, output: output);

            DeclareConsumer(
                queueName: queueName,
                handler: value
                    => output.WriteLine($"Received message: {value}"),
                output: output
            );

            PublishMessage(
                message: new(
                    ExchangeName: exchangeName,
                    Payload: new { Message = "Hello world!" }
                ),
                output: output
            );

            return Task.CompletedTask;
        }

        #endregion
    }
}
