using System.Text;
using System.Text.Json;

namespace Nameless.RabbitMQ.Course {
    public static class MessageHelper {
        #region Public Static Methods

        public static ReadOnlyMemory<byte> GetBytes(object value) {
            var json = JsonSerializer.Serialize(value);

            return Encoding.UTF8.GetBytes(json);
        }

        public static string GetString(ReadOnlyMemory<byte> buffer)
            => Encoding.UTF8.GetString(buffer.ToArray());

        public static string GetString(byte[] buffer)
            => Encoding.UTF8.GetString(buffer);

        #endregion
    }
}
