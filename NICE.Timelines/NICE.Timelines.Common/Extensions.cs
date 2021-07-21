using System;
using System.Text.Json;

namespace NICE.Timelines.Common
{
    public static class Extensions
    {
        public static T ToObject<T>(this JsonElement element)
        {
            var json = element.GetRawText();
            return JsonSerializer.Deserialize<T>(json);
        }

        public static string ToStringObject(this JsonElement element) //TODO: make the generic version work for all types..
        {
            if (element.ValueKind == JsonValueKind.Undefined)
                return null;

            var json = element.GetRawText();
            return JsonSerializer.Deserialize<string>(json);
        }

        public static DateTime? ToDateTime(this double millisecondsSinceUnixEpoch)
        {
            var unixEpochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return unixEpochDateTime.AddMilliseconds(millisecondsSinceUnixEpoch).ToLocalTime();
        }
    }
}