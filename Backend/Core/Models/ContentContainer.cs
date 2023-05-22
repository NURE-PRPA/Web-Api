using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models;

public class ContentContainer
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    [JsonConverter(typeof(JsonToByteArrayConverter))]
    public byte[] Binary { get; set; }
    public CourseModule Module { get; set; }
    public void InitializeEntity()
    {
        Id = Guid.NewGuid().ToString();
        Date = DateTime.Now;
    }

    public void RemoveCycles()
    {
        Module = null;
    }
}

internal sealed class JsonToByteArrayConverter : JsonConverter<byte[]?>
{
    // Converts base64 encoded string to byte[].
    public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.TryGetBytesFromBase64(out byte[]? result) || result == default)
        {
            throw new Exception("Add your fancy exception message here...");
        }
        return result;
    }

    // Converts byte[] to base64 encoded string.
    public override void Write(Utf8JsonWriter writer, byte[]? value, JsonSerializerOptions options)
    {
        writer.WriteBase64StringValue(value);
    }
}