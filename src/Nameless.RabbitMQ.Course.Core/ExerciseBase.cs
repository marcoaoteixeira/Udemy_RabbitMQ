using RabbitMQ.Client;

namespace Nameless.RabbitMQ.Course {
    public abstract class ExerciseBase {
        #region Public Abstract Properties

        public abstract string Code { get; }
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

        #region Public Abstract Methods

        public abstract Task RunAsync(TextWriter output, CancellationToken cancellationToken);

        #endregion
    }
}
