using System.ComponentModel;
using System.Reflection;

namespace Nameless.RabbitMQ.Course {
    public static class EnumExtension {
        #region Public Static Methods

        public static string GetDescription(this Enum self) {
            var enumType = self.GetType();
            var field = enumType.GetField(self.ToString());

            if (field is null) {
                return string.Empty;
            }

            var attr = field
                .GetCustomAttribute<DescriptionAttribute>(inherit: false);

            return attr is not null ? attr.Description : string.Empty;
        }

        #endregion
    }
}
