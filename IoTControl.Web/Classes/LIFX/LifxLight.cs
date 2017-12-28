using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IoTControl.Web.Classes.LIFX
{
    public partial class LifxLight
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("connected")]
        public bool Connected { get; set; }

        [JsonProperty("power")]
        public string Power { get; set; }

        [JsonProperty("color")]
        public LifxColor Color { get; set; }

        [JsonProperty("infrared")]
        public string Infrared { get; set; }

        [JsonProperty("brightness")]
        public double Brightness { get; set; }

        [JsonProperty("group")]
        public LifxGroup Group { get; set; }

        [JsonProperty("location")]
        public LifxLocation Location { get; set; }

        [JsonProperty("product")]
        public LifxProduct Product { get; set; }

        [JsonProperty("last_seen")]
        public DateTime LastSeen { get; set; }

        [JsonProperty("seconds_since_seen")]
        public long SecondsSinceSeen { get; set; }
    }

    public partial class LifxLight
    {
        public static List<LifxLight> FromJson(string json) => JsonConvert.DeserializeObject<List<LifxLight>>(json, Converter.Settings);

        public class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
            };
        }
    }
}