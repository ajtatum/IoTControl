using System.Collections.Generic;
using IoTControl.Web.Classes.LIFX;
using Newtonsoft.Json;

namespace IoTControl.Web.ExtensionMethods
{
    public static class LifxExtensions
    {
        public static string ToJson(this List<LifxLight> self) => JsonConvert.SerializeObject(self, LifxLight.Converter.Settings);
    }
}