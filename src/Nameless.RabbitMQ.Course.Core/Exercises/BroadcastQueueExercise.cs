using RabbitMQ.Client;

namespace Nameless.RabbitMQ.Course.Exercises {
    public sealed class BroadcastQueueExercise : ExerciseBase {
        #region Public Override Properties

        public override int Code => 3;
        public override string Description => "Broadcast (fanout) Queue Sample";

        #endregion

        #region Public Constructors

        public BroadcastQueueExercise(IModel channel)
            : base(channel) { }

        #endregion

        #region Public Override Methods

        public override Task RunAsync(TextWriter output, CancellationToken cancellationToken) {
            const string exchangeName = "ex.broadcast.exchange";
            DeclareExchange(
                exchangeName: exchangeName,
                type: ExchangeType.Fanout,
                output: output
            );

            const string queueNameA = "q.broadcast.queue.a";
            DeclareQueue(
                exchangeName: exchangeName,
                queueName: queueNameA,
                output: output
            );
            DeclareConsumer(
                queueName: queueNameA,
                handler: message => {
                    output.WriteLine("Hi! I'm consumer for queue A");
                    output.WriteLine($"Message: {message}");
                },
                output: output
            );

            const string queueNameB = "q.broadcast.queue.b";
            DeclareQueue(
                exchangeName: exchangeName,
                queueName: queueNameB,
                output: output
            );
            DeclareConsumer(
                queueName: queueNameB,
                handler: message => {
                    output.WriteLine("Hi! I'm consumer for queue B");
                    output.WriteLine($"Message: {message}");
                    output.WriteLine();
                },
                output: output
            );

            PublishMessage(
                message: new(
                    ExchangeName: exchangeName,
                    Payload: new { Message = "!!!BROADCAST!!!" }
                ),
                output: output
            );

            return Task.CompletedTask;
        }        

        #endregion
    }
}
