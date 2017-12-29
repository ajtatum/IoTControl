using System;
using Newtonsoft.Json;

namespace IoTControl.Web.Classes.Lighting
{
    public class HsvColor
    {
        private double _h;
        private double _s;
        private double _v;

        /// <summary>
        /// Hue
        /// </summary>
        [JsonProperty("h")]
        public double H
        {
            get => _h;
            set => _h = Math.Round(value,0);
        }

        /// <summary>
        /// Saturation
        /// </summary>
        [JsonProperty("s")]
        public double S
        {
            get => _s;
            set => _s = Math.Round(value,2);
        }

        /// <summary>
        /// Brightness
        /// </summary>
        [JsonProperty("v")]
        public double V
        {
            get => _v;
            set => _v = Math.Round(value,2);
        }

        /// <summary>
        /// Alpha or Opacity
        /// </summary>
        [JsonProperty("a")]
        public double A { get; set; }
    }
}