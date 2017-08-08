using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Interfaces
{
    public interface IEmotionService : IAuroraService
    {
        void SetEmotion(EmotionData emotion);

        List<EmotionDto> GetEmotions(DateTime? fromDate = null, DateTime? toDate = null);
    }
}
