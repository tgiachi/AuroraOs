using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Data.Errors;
using AuroraOs.Common.Core.Data.IoT.Base;
using AuroraOs.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Interfaces
{
    public interface ISensorsService : IAuroraService
    {
        List<SensorError> GetSensorErrors();

        List<BaseIot> GetSensors();

        GraphData GetDataSensor(string sensorName, DateTime fromDate, DateTime toDate);


        bool AddSensorValue(string sensorName, string type, object data);

    }
}
