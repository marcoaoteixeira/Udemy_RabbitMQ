using System.Text;
using System.Text.Json;

namespace Nameless.RabbitMQ.Course {
    public static class PayloadHelper {
        #region Public Static Methods

        public static ReadOnlyMemory<byte> GetBytes(object payload)
            => Encoding.UTF8.GetBytes(
                s: JsonSerializer.Serialize(payload)
            );

        public static string GetString(ReadOnlyMemory<byte> buffer)
            => Encoding.UTF8.GetString(
                bytes: buffer.ToArray()
            );

        #endregion
    }
}
