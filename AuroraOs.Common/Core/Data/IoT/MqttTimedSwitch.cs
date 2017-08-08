namespace AuroraOs.Common.Core.Data.IoT
{
    public class MqttTimedSwitch : MqttSwitch
    {
        public long Seconds { get; set; } = 1000 * 60;
    }
}
