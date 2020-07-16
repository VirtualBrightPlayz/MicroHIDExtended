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
            Exiled.Events.Handlers.Server.RoundStarted += PLEV.RoundStart;
            inst = new Harmony("virtualbrightplayz.mhidext");
            inst.PatchAll();
        }
    }
}
