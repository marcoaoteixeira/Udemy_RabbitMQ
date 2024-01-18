using Autofac;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Nameless.RabbitMQ.Course {
    public sealed class RabbitMQModule : Module {
        #region Private Constants

        private const string CONNECTION_FACTORY_TOKEN = $"{nameof(IConnectionFactory)}::9bbda99a-3ee1-45a1-a72b-b603d92d4f87";
        private const string CONNECTION_TOKEN = $"{nameof(IConnection)}::aef0a325-356b-4d8b-a8c8-31fcaecd518c";
        private const string CONFIG_SECTION_NAME = "RabbitMQ";

        #endregion

        #region Protected Override Methods

        protected override void Load(ContainerBuilder builder) {
            builder
                .Register(ConnectionFactoryResolver)
                .Named<IConnectionFactory>(CONNECTION_FACTORY_TOKEN)
                .SingleInstance();

            builder
                .Register(ConnectionResolver)
                .Named<IConnection>(CONNECTION_TOKEN)
                .SingleInstance();

            builder
                .Register(ChannelResolver)
                .As<IModel>()
                .SingleInstance();

            base.Load(builder);
        }

        #endregion

        #region Private Static Methods

        private static IConnectionFactory ConnectionFactoryResolver(IComponentContext ctx) {
            var configuration = ctx
                .ResolveOptional<IConfiguration>();
            var options = configuration is not null
                ? configuration
                    .GetSection(CONFIG_SECTION_NAME)
                    .Get<RabbitMQOptions>() ?? RabbitMQOptions.Default
                : RabbitMQOptions.Default;

            var connectionFactory = new ConnectionFactory {
                HostName = options.Server.Hostname,
                Port = options.Server.Port,
                UserName = options.Server.Username,
                Password = options.Server.Password
            };

            if (options.Server.UseSsl) {
                connectionFactory.Ssl = new(
                    serverName: options.Server.ServerName,
                    certificatePath: options.Server.CertificatePath,
                    enabled: true
                );
            }

            return connectionFactory;
        }

        private static IConnection ConnectionResolver(IComponentContext ctx) {
            var connectionFactory = ctx
                .ResolveNamed<IConnectionFactory>(CONNECTION_FACTORY_TOKEN);

            var connection = connectionFactory.CreateConnection();

            return connection;
        }

        private static IModel ChannelResolver(IComponentContext ctx) {
            var connection = ctx
                .ResolveNamed<IConnection>(CONNECTION_TOKEN);

            return connection.CreateModel();
        }

        #endregion
    }
}
