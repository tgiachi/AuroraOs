using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Entities.Core.Repositories.Interfaces
{
    public interface ISensorValuesRepository : IAuroraService
    {
        void AddData(string sensorName, string unitOfMeasurement, string value);

        GraphData GetSensorData(string sensorName, DateTime? fromDateTime = null, DateTime? toDateTime = null);

        List<SensorAvailableDto> GetAvaiablesSensorList();

        GraphData GetLastSensorData(string sensorName, string um = null);
    }
}
