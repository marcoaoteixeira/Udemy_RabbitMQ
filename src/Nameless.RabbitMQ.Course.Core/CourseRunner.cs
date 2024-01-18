using System.Reflection;
using Autofac;

namespace Nameless.RabbitMQ.Course {
    public sealed class CourseRunner : IDisposable {
        #region Private Read-Only Fields

        private readonly Assembly[] _supportAssemblies;

        #endregion

        #region Private Fields

        private IContainer? _container;
        private bool _disposed;

        #endregion

        #region Public Constructors

        public CourseRunner(Assembly[] supportAssemblies) {
            _supportAssemblies = supportAssemblies ?? throw new ArgumentNullException(nameof(supportAssemblies));
        }

        #endregion

        #region Private Methods

        private ContainerBuilder ConfigureContainerBuilder() {
            var builder = new ContainerBuilder();

            builder
                .RegisterModule<RabbitMQModule>()
                .RegisterModule(new ExerciseModule(_supportAssemblies));

            return builder;
        }

        private IContainer GetContainer()
            => _container ??= ConfigureContainerBuilder().Build();

        private void BlockAccessAfterDispose() {
            if (_disposed) {
                throw new ObjectDisposedException(nameof(CourseRunner));
            }
        }

        private void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }

            if (disposing) {
                _container?.Dispose();
            }

            _container = null;
            _disposed = true;
        }

        #endregion

        #region Public Methods

        public ExerciseBase[] GetExercises()
            => GetContainer()
                .Resolve<ExerciseBase[]>()
                .OrderBy(exercise => exercise.Code)
                .ToArray();

        #endregion

        #region IDisposable

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
