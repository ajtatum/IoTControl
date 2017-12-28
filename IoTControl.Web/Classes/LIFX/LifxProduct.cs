using Newtonsoft.Json;

namespace IoTControl.Web.Classes.LIFX
{
    public class LifxProduct
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("capabilities")]
        public LifxCapabilities Capabilities { get; set; }
    }
}