namespace AuroraOs.Common.Core.Data.IoT.Base
{
    public class BaseIot
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Room { get; set; }

        public bool IsEnabled { get; set; } = true;

        public override string ToString()
        {
            return $"ClassType = {GetType().Name} - Type {Type} - Name: {Name} in Room {Room} - IsEnabled: {IsEnabled}";
        }
    }
}
