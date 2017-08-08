using Newtonsoft.Json;

namespace AuroraOs.Common.Core.Data
{
    public class SunsetData
    {
        [JsonProperty("results")]
        public SunsetDataResults Results { get; set; }

        public string Status { get; set; }
    }
}