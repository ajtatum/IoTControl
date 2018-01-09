using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace IoTControl.Web.Classes.LIFX
{
    public class LifxColor
    {
        public LifxColor() { }

        public LifxColor(Color color)
        {
            Hue = color.GetHue();
            Saturation = color.GetSaturation();
            Brightness = color.GetBrightness();
        }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("hue", NullValueHandling = NullValueHandling.Ignore)]
        public double? Hue { get; set; }

        [JsonProperty("saturation", NullValueHandling = NullValueHandling.Ignore)]
        public double? Saturation { get; set; }

        [JsonProperty("brightness", NullValueHandling = NullValueHandling.Ignore)]
        public double? Brightness { get; set; }

        [JsonProperty("kelvin", NullValueHandling = NullValueHandling.Ignore)]
        public long? Kelvin { get; set; }

        [JsonProperty("hex", NullValueHandling = NullValueHandling.Ignore)]
        public string Hex { get; set; }

        [JsonProperty("rgb", NullValueHandling = NullValueHandling.Ignore)]
        public string RGB { get; set; }

        public override string ToString()
        {
            var properties = new List<string>();

            if (Name != null)
                return Name;
            if (Hex != null)
                return Hex;
            if (RGB != null)
                return RGB;
            if (Hue != null)
                properties.Add($"hue:{Hue}");
            if(Saturation != null)
                properties.Add($"saturation:{Saturation}");
            if (Brightness != null)
                properties.Add($"brightness:{Brightness}");
            if (Kelvin != null)
                properties.Add($"kelvin:{Kelvin}");

            return string.Join(" ", properties);
        }
    }
}