using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Engine.Core.Interfaces;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Services
{

    [AuroraService("Health")]
    public class EmotionService : IEmotionService
    {

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly ISensorValuesRepository _sensorValuesRepository;

        public EmotionService(ISensorValuesRepository sensorValuesRepository)
        {
            _sensorValuesRepository = sensorValuesRepository;
        }

        public Task Init()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }

        public List<EmotionDto> GetEmotions(DateTime? fromDate = default(DateTime?), DateTime? toDate = default(DateTime?))
        {
            var list = _sensorValuesRepository.GetSensorData("emotion", null, null);
            var emotionList = new List<EmotionDto>();

            list.Values.ForEach(x =>
            {
                var em = EmotionData.Normal;

                Enum.TryParse(x.Data, out em);
                emotionList.Add(new EmotionDto()
                {
                    Data = em,
                    DateTime = x.LastUpdate
                });
            });

            return emotionList;
        }

        public void SetEmotion(EmotionData emotion)
        {
            _sensorValuesRepository.AddData("emotion", "emotionData", emotion.ToString());
        }
    }
}
