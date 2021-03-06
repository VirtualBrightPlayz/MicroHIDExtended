﻿using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace MicroHIDExtended
{
    [HarmonyPatch(typeof(Pickup), nameof(Pickup.LateUpdate))]
    public class PickupUpdatePatch
    {
        public static Dictionary<Pickup, float> timers = new Dictionary<Pickup, float>();

        public static void Postfix(Pickup __instance)
        {
            if (__instance.ItemId == ItemType.MicroHID)
            {
                if (!timers.ContainsKey(__instance))
                    timers.Add(__instance, 0f);
                timers[__instance] += Time.deltaTime;
                if (timers[__instance] >= MicroHIDPlugin.instance.Config.mhid_charge_interval)
                {
                    timers[__instance] = 0f;
                    __instance.durability += MicroHIDPlugin.instance.Config.mhid_charge_rate;
                    //__instance.RefreshDurability();
                }
            }
        }
    }
}
