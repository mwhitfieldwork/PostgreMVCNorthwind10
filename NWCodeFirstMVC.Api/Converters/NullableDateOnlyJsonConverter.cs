using System.Text.Json;
using System.Text.Json.Serialization;

namespace NWCodeFirstMVC.Api.Converters
{
    public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
    {
        public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value is null ? null : DateOnly.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
            else
                writer.WriteNullValue();
        }
    }
}
