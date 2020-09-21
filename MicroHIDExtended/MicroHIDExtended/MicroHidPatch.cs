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
            __instance.chargeupTime = MicroHIDPlugin.instance.Config.chargeupTime;
            __instance.chargedownTime = MicroHIDPlugin.instance.Config.chargedownTime;
            if (MicroHIDPlugin.instance.Config.disablePreDischarge)
                __instance.soundEffectPause = 1.5f;
            if (!timers.ContainsKey(__instance))
                timers.Add(__instance, 0f);
            if (__instance.refHub.inventory.curItem == ItemType.MicroHID && __instance.NetworkCurrentHidState == MicroHID.MicroHidState.Idle)
            {
                timers[__instance] += Time.deltaTime;
                if (timers[__instance] >= MicroHIDPlugin.instance.Config.mhid_charge_interval)
                {
                    timers[__instance] = 0f;
                    __instance.ChangeEnergy(__instance.GetEnergy() + MicroHIDPlugin.instance.Config.mhid_charge_rate);
                    __instance.NetworkEnergy = __instance.GetEnergy();
                }
            }
            if (__instance.refHub.inventory.curItem == ItemType.MicroHID && __instance.NetworkCurrentHidState == MicroHID.MicroHidState.Discharge)
            {
                timers[__instance] += Time.deltaTime;
                if (timers[__instance] >= MicroHIDPlugin.instance.Config.mhid_charge_use_interval)
                {
                    timers[__instance] = 0f;
                    __instance.ChangeEnergy(__instance.GetEnergy() + MicroHIDPlugin.instance.Config.mhid_charge_use_rate);
                    __instance.NetworkEnergy = __instance.GetEnergy();
                }
            }
            if (__instance.refHub.inventory.curItem == ItemType.MicroHID && __instance.NetworkCurrentHidState == MicroHID.MicroHidState.Spinning)
            {
                timers[__instance] += Time.deltaTime;
                if (timers[__instance] >= MicroHIDPlugin.instance.Config.mhid_charge_use_idle_interval)
                {
                    timers[__instance] = 0f;
                    __instance.ChangeEnergy(__instance.GetEnergy() + MicroHIDPlugin.instance.Config.mhid_charge_use_idle_rate);
                    __instance.NetworkEnergy = __instance.GetEnergy();
                }
            }
        }
    }
}
