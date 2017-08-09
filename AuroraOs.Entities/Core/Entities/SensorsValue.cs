using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Entities.Core.Entities
{
    [MongoDocument("sensorsValues")]
    public class SensorsValue : BaseNoSqlEntity
    { 
        public string SensorName { get; set; }

        public string UnitOfMeasurement { get; set; }

        public DateTime EventDateTime { get; set; }

        public string Value { get; set; }
    }
}
