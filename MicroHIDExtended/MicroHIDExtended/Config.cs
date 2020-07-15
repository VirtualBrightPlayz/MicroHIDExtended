using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroHIDExtended
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public float mhid_charge_rate { get; set; } = 0.015f;
        public float mhid_charge_interval { get; set; } = 0.5f;
        public float mhid_charge_use_rate { get; set; } = 0.015f;
        public float mhid_charge_use_interval { get; set; } = 0.5f;

    }
}
