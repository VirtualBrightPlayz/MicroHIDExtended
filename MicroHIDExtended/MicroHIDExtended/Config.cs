using Exiled.API.Interfaces;

namespace MicroHIDExtended
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public int mhid_damage_per_hit { get; set; } = -1;
        public float mhid_charge_rate { get; set; } = 0.015f;
        public float mhid_charge_interval { get; set; } = 0.5f;
        public float mhid_charge_use_rate { get; set; } = 0.015f;
        public float mhid_charge_use_interval { get; set; } = 0.5f;
    }
}
