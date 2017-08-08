using System;
using Newtonsoft.Json;
namespace AuroraOs.Common.Core.Data.Dto
{
    /// <summary>
    /// Location GPS
    /// </summary>
    public class OwnTracksLocation
    {
        [JsonProperty("_type")]
        public string Type { get; set; }

        [JsonProperty("acc")]
        public int Accuracy { get; set; }

        [JsonProperty("alt")]
        public int Altitude {get;set;}

        [JsonProperty("batt")]
        public int Battery {get;set;}
        
        [JsonProperty("lat")]
        public double Latitude {get;set;}

        [JsonProperty("lon")]
        public double Longitude {get;set;}

        [JsonProperty("rad")]
        public int Radius {get;set;}
        
        [JsonProperty("t")]
        public string Trigger {get;set;}

        [JsonProperty("tid")]
        public string TrackerId {get;set;}

        [JsonProperty("tst")]

        public string Timestamp {get;set;}
    }
}