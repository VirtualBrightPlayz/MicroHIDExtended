using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
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
                if (timers[__instance] >= MicroHIDPlugin.instance.Config.mhid_charge_rate)
                {
                    timers[__instance] = 0f;
                    __instance.ChangeEnergy(__instance.GetEnergy() + MicroHIDPlugin.instance.Config.mhid_charge_rate);
                    __instance.NetworkEnergy = __instance.GetEnergy();
                }
            }
            if (__instance.refHub.inventory.curItem == ItemType.MicroHID && __instance.NetworkCurrentHidState != MicroHID.MicroHidState.Idle)
            {
                timers[__instance] += Time.deltaTime;
                if (timers[__instance] >= MicroHIDPlugin.instance.Config.mhid_charge_use_interval)
                {
                    timers[__instance] = 0f;
                    __instance.ChangeEnergy(__instance.GetEnergy() + MicroHIDPlugin.instance.Config.mhid_charge_use_rate);
                    __instance.NetworkEnergy = __instance.GetEnergy();
                }
            }
        }
    }
}
