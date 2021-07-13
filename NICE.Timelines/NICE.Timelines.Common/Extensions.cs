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
    }
}