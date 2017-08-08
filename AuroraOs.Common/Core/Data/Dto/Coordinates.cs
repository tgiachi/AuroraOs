using System;
using System.Collections.Generic;
using System.Text;

namespace AuroraOs.Common.Core.Data.Dto
{
    public class Coordinates
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
