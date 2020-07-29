using System.Collections.Generic;
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
                if (timers[__instance] >= MicroHIDPlugin.instance.Config.mhid_charge_interval)
                {
                    Exiled.API.Features.Log.Warn("recharching: timer = " + timers[__instance]);
                    timers[__instance] = 0f;
                    __instance.ChangeEnergy(__instance.GetEnergy() + MicroHIDPlugin.instance.Config.mhid_charge_rate);
                    __instance.Energy = __instance.GetEnergy();
                    __instance.NetworkEnergy = __instance.GetEnergy();
                    Exiled.API.Features.Log.Warn("energy = " + __instance.GetEnergy());
                }
            }
            if (__instance.refHub.inventory.curItem == ItemType.MicroHID && (__instance.NetworkCurrentHidState == MicroHID.MicroHidState.Discharge || __instance.NetworkCurrentHidState == MicroHID.MicroHidState.Spinning))
            {
                timers[__instance] += Time.deltaTime;
                if (timers[__instance] >= MicroHIDPlugin.instance.Config.mhid_charge_use_interval)
                {
                    timers[__instance] = 0f;
                    //__instance.ChangeEnergy(__instance.GetEnergy() + MicroHIDPlugin.instance.Config.mhid_charge_use_rate);
                    __instance.Energy = __instance.GetEnergy();
                    __instance.NetworkEnergy = __instance.GetEnergy();
                }
            }
        }
    }
}
