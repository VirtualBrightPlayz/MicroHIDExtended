using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXILED;
using EXILED.Extensions;
using Harmony;
using UnityEngine;

namespace MicroHIDExtended
{
    [HarmonyPatch(typeof(MicroHID), nameof(MicroHID.UpdateServerside))]
    public class MicroHidUpdatePatch
    {
        public static Dictionary<MicroHID, float> timers = new Dictionary<MicroHID, float>();

        public static void Prefix(MicroHID __instance)
        {
            if (!timers.ContainsKey(__instance))
                timers.Add(__instance, 0f);
            if (__instance.refHub.inventory.curItem == ItemType.MicroHID && __instance.NetworkCurrentHidState == MicroHID.MicroHidState.Idle)
            {
                timers[__instance] += Time.deltaTime;
                if (timers[__instance] >= MicroHIDPlugin.chargeIntervals)
                {
                    timers[__instance] = 0f;
                    __instance.ChangeEnergy(__instance.GetEnergy() + MicroHIDPlugin.chargeRate);
                    __instance.NetworkEnergy = __instance.GetEnergy();
                }
            }
            if (__instance.refHub.inventory.curItem == ItemType.MicroHID && __instance.NetworkCurrentHidState != MicroHID.MicroHidState.Idle)
            {
                timers[__instance] += Time.deltaTime;
                if (timers[__instance] >= MicroHIDPlugin.useChargeIntervals)
                {
                    timers[__instance] = 0f;
                    __instance.ChangeEnergy(__instance.GetEnergy() + MicroHIDPlugin.useChargeRate);
                    __instance.NetworkEnergy = __instance.GetEnergy();
                }
            }
        }
    }
}
