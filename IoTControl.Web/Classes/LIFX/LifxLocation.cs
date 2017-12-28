using Newtonsoft.Json;

namespace IoTControl.Web.Classes.LIFX
{
    public class LifxLocation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}