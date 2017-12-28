using Newtonsoft.Json;

namespace IoTControl.Web.Classes.LIFX
{
    public class LifxColor
    {
        [JsonProperty("hue")]
        public double Hue { get; set; }

        [JsonProperty("saturation")]
        public double Saturation { get; set; }

        [JsonProperty("kelvin")]
        public long Kelvin { get; set; }
    }
}