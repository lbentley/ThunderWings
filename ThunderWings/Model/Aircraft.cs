using System.Text.Json.Serialization;

namespace ThunderWings.Model
{
    public class Aircraft
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("topSpeed")]
        public int TopSpeed { get; set; }

        [JsonPropertyName("price")]
        public long Price { get; set; }
    }
}

