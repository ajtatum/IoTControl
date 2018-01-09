using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using IoTControl.Models;
using Newtonsoft.Json;

namespace IoTControl.Web.ViewModels
{
    public class LifxViewModel
    {
        public class FavoriteEditor : IoTControl.Models.UserLifxFavorite
        {
            [NotMapped]
            public List<SelectListItem> SelectorTypeList { get; set; }

            [NotMapped]
            public List<SelectListItem> PowerOptionsList { get; set; }

            [NotMapped]
            public List<Kelvin> KelvinList { get; set; }

            [NotMapped]
            [Display(Name = "Color Picker")]
            public string ColorPicker { get; set; }

            [NotMapped]
            public string Kelvin { get; set; }

            [NotMapped]
            public LifxFavoriteJson LifxFavoriteJson { get; set; }
        }

        public class LifxFavoriteJson
        {
            [JsonProperty("power", NullValueHandling = NullValueHandling.Ignore)]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Please select if you want to turn the lights on or off.")]
            public string Power { get; set; }

            [JsonProperty("hue", NullValueHandling = NullValueHandling.Ignore)]
            public int? Hue { get; set; }

            [JsonProperty("saturation", NullValueHandling = NullValueHandling.Ignore)]
            public double? Saturation { get; set; }

            [JsonProperty("brightness", NullValueHandling = NullValueHandling.Ignore)]
            public double? Brightness { get; set; }

            [JsonProperty("kelvin", NullValueHandling = NullValueHandling.Ignore)]
            public long? Kelvin { get; set; }

            [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
            public int Duration { get; set; }
        }
    }
}