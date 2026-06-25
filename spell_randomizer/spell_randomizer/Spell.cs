using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace spell_randomizer;

[JsonConverter(typeof(SpellConverter))]
public class Spell
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }

    public Spell(string name, int level, string url)
    {
        Name = name;
        Level = level;
        Url = url;
        Description = ""; // Loaded on demand
    }

    public override string ToString()
    {
        return Level == 0 ? Name : $"{Name} - Spell level {Level}";
    }
}

public class SpellConverter : JsonConverter<Spell>
{
    public override Spell Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            // Backwards compatibility with old save files that stored strings
            string? spellString = reader.GetString();
            if (spellString != null && spellString.Contains(" - Spell level "))
            {
                var parts = spellString.Split(new[] { " - Spell level " }, StringSplitOptions.None);
                int level = 0;
                int.TryParse(parts[1], out level);
                return new Spell(parts[0], level, "");
            }
            return new Spell(spellString ?? "Unknown Spell", 0, "");
        }

        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            
            string GetStringProperty(string propertyName)
            {
                if (root.TryGetProperty(propertyName, out var prop)) return prop.GetString() ?? "";
                if (root.TryGetProperty(propertyName.ToLower(), out prop)) return prop.GetString() ?? "";
                return "";
            }

            int GetIntProperty(string propertyName)
            {
                if (root.TryGetProperty(propertyName, out var prop)) return prop.GetInt32();
                if (root.TryGetProperty(propertyName.ToLower(), out prop)) return prop.GetInt32();
                return 0;
            }

            string name = GetStringProperty("Name");
            int level = GetIntProperty("Level");
            string description = GetStringProperty("Description");
            string url = GetStringProperty("Url");

            var spell = new Spell(name, level, url) { Description = description };
            return spell;
        }
    }

    public override void Write(Utf8JsonWriter writer, Spell value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name);
        writer.WriteNumber("level", value.Level);
        writer.WriteString("description", value.Description);
        writer.WriteString("url", value.Url);
        writer.WriteEndObject();
    }
}
