namespace Nameless.RabbitMQ.Course {
    public sealed record Message(string ExchangeName, object Payload, string? RoutingKey = null);
}
