using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXILED;
using EXILED.Extensions;
using Harmony;

namespace MicroHIDExtended
{
    public class MicroHIDPlugin : Plugin
    {
        public override string getName => "MicroHIDExtended";
        public PluginEvents PLEV;
        public static float chargeRate;
        public static float chargeIntervals;
        public static float useChargeRate;
        public static float useChargeIntervals;
        public HarmonyInstance inst;

        public override void OnDisable()
        {
            if (!Config.GetBool("mhid_enable", true))
                return;
            Events.RoundStartEvent -= PLEV.RoundStart;
            PLEV = null;
            inst.UnpatchAll();
            inst = null;
        }

        public override void OnEnable()
        {
            if (!Config.GetBool("mhid_enable", true))
                return;
            PLEV = new PluginEvents(this);
            Events.RoundStartEvent += PLEV.RoundStart;
            chargeRate = Config.GetFloat("mhid_charge_rate", 0.015f);
            chargeIntervals = Config.GetFloat("mhid_charge_interval", 0.5f);
            useChargeRate = Config.GetFloat("mhid_charge_use_rate", 0.015f);
            useChargeIntervals = Config.GetFloat("mhid_charge_use_interval", 0.5f);
            inst = HarmonyInstance.Create("virtualbrightplayz.mhidext");
            inst.PatchAll();
        }

        public override void OnReload()
        {
        }
    }
}
