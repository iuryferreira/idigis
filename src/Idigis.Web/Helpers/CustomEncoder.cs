using System;
using System.Text;
using System.Text.Json;

namespace Idigis.Web.Helpers
{
    public static class CustomEncoder
    {
        public static string Encode<T> (T value)
        {
            if (value is null)
            {
                return string.Empty;
            }

            var serialized = JsonSerializer.Serialize(value);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serialized));
        }

        public static T Decode<T> (string value)
        {
            var bytes = Convert.FromBase64String(value);
            var decoded = Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<T>(decoded);
        }
    }
}
