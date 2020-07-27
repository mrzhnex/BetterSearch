using Exiled.API.Interfaces;

namespace BetterSearch
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool IsFullRp { get; set; } = false;
    }
}