using Newtonsoft.Json;

namespace IoTControl.Web.Classes.LIFX
{
    public class LifxCapabilities
    {
        [JsonProperty("has_color")]
        public bool HasColor { get; set; }

        [JsonProperty("has_variable_color_temp")]
        public bool HasVariableColorTemp { get; set; }

        [JsonProperty("has_ir")]
        public bool HasIr { get; set; }

        [JsonProperty("has_multizone")]
        public bool HasMultizone { get; set; }

        [JsonProperty("min_kelvin")]
        public long MinKelvin { get; set; }

        [JsonProperty("max_kelvin")]
        public long MaxKelvin { get; set; }
    }
}