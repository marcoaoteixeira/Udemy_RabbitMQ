using System.Reflection;
using Autofac;

namespace Nameless.RabbitMQ.Course {
    public sealed class ExerciseModule : Autofac.Module {
        #region Private Read-Only Fields

        private readonly Assembly[] _supportAssemblies;

        #endregion

        #region Public Constructors

        public ExerciseModule(Assembly[] supportAssemblies) {
            _supportAssemblies = supportAssemblies ?? throw new ArgumentNullException(nameof(supportAssemblies));
        }

        #endregion

        #region Protected Override Methods

        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterAssemblyTypes(_supportAssemblies)
                .Where(type => type.BaseType == typeof(ExerciseBase))
                .As<ExerciseBase>()
                .InstancePerDependency();

            base.Load(builder);
        }

        #endregion
    }
}
