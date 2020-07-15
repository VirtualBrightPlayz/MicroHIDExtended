using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API;
using Exiled.API.Features;
using Exiled.Events;
using Exiled;
using HarmonyLib;

namespace MicroHIDExtended
{
    public class MicroHIDPlugin : Plugin<Config>
    {
        public override string Name => "MicroHIDExtended";
        public PluginEvents PLEV;
        public static float chargeRate;
        public static float chargeIntervals;
        public static float useChargeRate;
        public static float useChargeIntervals;
        public Harmony inst;

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= PLEV.RoundStart;
            base.OnDisabled();
            PLEV = null;
            inst.UnpatchAll();
            inst = null;
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            PLEV = new PluginEvents(this);
            Exiled.Events.Handlers.Server.RoundStarted += PLEV.RoundStart;
            inst = new Harmony("virtualbrightplayz.mhidext");
            inst.PatchAll();
        }
    }
}
