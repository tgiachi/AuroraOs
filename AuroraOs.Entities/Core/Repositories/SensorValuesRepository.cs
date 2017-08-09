﻿using AuroraOs.Entities.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Enums;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Entities.Core.Entities;
using MoreLinq;

namespace AuroraOs.Entities.Core.Repositories
{
    [AuroraService(AuroraServiceType.PerRequest)]
    public class SensorValuesRepository : ISensorValuesRepository
    {
        private INoSqlService _dbContext; 

        public SensorValuesRepository(INoSqlService noSqlService)
        {
            _dbContext = noSqlService;
        }

        public void AddData(string sensorName, string unitOfMeasurement, string value)
        {
            _dbContext.Insert(new SensorsValue()
            {
                EventDateTime = DateTime.Now,
                SensorName = sensorName,
                UnitOfMeasurement = unitOfMeasurement,
                Value = value
            });

        }

        public void Dispose()
        {
         
        }

        public List<SensorAvailableDto> GetAvaiablesSensorList()
        {
            var sens = _dbContext.SelectAll<SensorsValue>().OrderByDescending(value => value.EventDateTime).DistinctBy(value => value.SensorName).ToList();
            var dc = new List<SensorAvailableDto>();

            sens.ForEach(value =>
            {
                dc.Add(new SensorAvailableDto()
                {
                    Name = value.SensorName,
                    LastUpdate = value.EventDateTime
                });
            });


            return dc;



        }

        public GraphData GetLastSensorData(string sensorName, string um = null)
        {
            var sens = _dbContext.SelectAll<SensorsValue>().Where(value => value.SensorName == sensorName).OrderByDescending(s => s.EventDateTime).FirstOrDefault();

            if (sens != null)
            {
                var gh = new GraphData()
                {
                    SensorName = sens.SensorName,


                };


                gh.Values.Add(new GraphDataDetail()
                {
                    Data = sens.Value,
                    LastUpdate = sens.EventDateTime,
                    UnitOfMeasurement = sens.UnitOfMeasurement
                });


                return gh;

            }

            return new GraphData();
        }

        public GraphData GetSensorData(string sensorName, DateTime? fromDateTime = default(DateTime?), DateTime? toDateTime = default(DateTime?))
        {
            var data = _dbContext.SelectAll<SensorsValue>().Where(value => value.SensorName == sensorName).ToList();

            if (fromDateTime.HasValue)
                data.Where(s => s.EventDateTime <= fromDateTime.Value);

            if (toDateTime.HasValue)
                data.Where(s => s.EventDateTime >= toDateTime.Value);


            var sens = data.ToList();
            if (sens != null)
            {

                var gh = new GraphData()
                {
                    SensorName = sens.FirstOrDefault().SensorName
                };

                foreach (var sensorValue in sens)
                {
                    //sensorValue.EventDateTime.ToString("O"), sensorValue.Value
                    gh.Values.Add(new GraphDataDetail()
                    {
                        UnitOfMeasurement = sensorValue.UnitOfMeasurement,
                        Data = sensorValue.Value,
                        LastUpdate = sensorValue.EventDateTime
                    });
                }

                return gh;

            }

            return null;
        }
    }
}