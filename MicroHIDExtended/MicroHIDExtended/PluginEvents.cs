using MEC;
using System;
using System.Collections.Generic;

namespace MicroHIDExtended
{
    public class PluginEvents
    {
        private MicroHIDPlugin plugin;
        private CoroutineHandle handle;

        public PluginEvents(MicroHIDPlugin microHIDPlugin)
        {
            this.plugin = microHIDPlugin;
            //handle = Timing.RunCoroutine(ChargeMicro(), Segment.FixedUpdate);
        }

        ~PluginEvents()
        {
            //Timing.KillCoroutines(handle);
        }

        internal void RoundStart()
        {
            MicroHidUpdatePatch.timers.Clear();
            PickupUpdatePatch.timers.Clear();
            InventoryPatch.timers.Clear();
        }

        private IEnumerator<float> ChargeMicro()
        {
            while (true)
            {
                yield return Timing.WaitForOneFrame;
                
            }
        }
    }
}