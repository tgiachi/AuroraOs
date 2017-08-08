using Newtonsoft.Json;

namespace AuroraOs.Common.Core.Data
{
    public class SunsetDataResults
    {
        [JsonProperty("sunrise")]
        public string Sunrise { get; set; }

        [JsonProperty("sunset")]
        public string Sunset { get; set; }

        [JsonProperty("solar_noon")]
        public string SolarNoon {get;set;}

        [JsonProperty("day_length")]
        public string DayLength {get;set;}

    }
}