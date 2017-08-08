using System;

namespace AuroraOs.Common.Core.Data.Dto
{
    public class EmotionDto
    {
        public DateTime DateTime {get;set;}

        public EmotionData Data {get;set;}


        public override string ToString()
        {
            return $"{DateTime} - {Data}";

        }
    }
}