using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DefenceDB.DAL.Config;

public class StringDictionaryConverter : JsonConverter<Dictionary<string, string>>
{
    public override Dictionary<string, string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        var dictionary = new Dictionary<string, string>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return dictionary;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string propertyName = reader.GetString() ?? string.Empty;
            reader.Read();

            string? value = null;
            if (reader.TokenType == JsonTokenType.String)
            {
                value = reader.GetString();
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                value = reader.GetDouble().ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (reader.TokenType == JsonTokenType.True)
            {
                value = "true";
            }
            else if (reader.TokenType == JsonTokenType.False)
            {
                value = "false";
            }
            else if (reader.TokenType == JsonTokenType.Null)
            {
                value = null;
            }
            else
            {
                using var document = JsonDocument.ParseValue(ref reader);
                value = document.RootElement.ToString();
            }

            if (value != null)
            {
                dictionary[propertyName] = value;
            }
        }
        return dictionary;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, string> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var kvp in value)
        {
            writer.WriteString(kvp.Key, kvp.Value);
        }
        writer.WriteEndObject();
    }
}
