using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace IoTControl.Web.Models
{
    public class LifxStateResponse
    {
        [JsonProperty(PropertyName = "results")]
        public List<Result> Results { get; set; }

        public class Result
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
            [JsonProperty(PropertyName = "label")]
            public string Label { get; set; }
        }
    }
}