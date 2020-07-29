using System;
using Exiled.API.Features;
using HarmonyLib;

namespace MicroHIDExtended
{
    public class MicroHIDPlugin : Plugin<Config>
    {
        public static MicroHIDPlugin instance;
        public override string Name => "MicroHIDExtended";
        public override string Author => "VirtualBrightPlayz";
        public override Version Version => new Version(1, 1, 0);
        public PluginEvents PLEV;
        public Harmony inst;

        public override void OnDisabled()
        {
            base.OnDisabled();
            Exiled.Events.Handlers.Server.RoundStarted -= PLEV.RoundStart;
            PLEV = null;
            inst.UnpatchAll();
            inst = null;
            instance = null;
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            instance = this;
            PLEV = new PluginEvents(this);
            Log.Warn(Config.mhid_charge_rate);
            Exiled.Events.Handlers.Server.RoundStarted += PLEV.RoundStart;
            inst = new Harmony("virtualbrightplayz.mhidext");
            inst.PatchAll();
        }
    }
}
