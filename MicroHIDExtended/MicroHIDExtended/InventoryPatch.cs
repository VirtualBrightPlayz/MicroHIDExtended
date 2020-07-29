﻿using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace MicroHIDExtended
{
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.Update))]
    public class InventoryPatch
    {
        public static Dictionary<Inventory, float> timers = new Dictionary<Inventory, float>();

        public static void Prefix(Inventory __instance)
        {
            if (!timers.ContainsKey(__instance))
                timers.Add(__instance, 0f);
            timers[__instance] += Time.deltaTime;
            if (timers[__instance] >= MicroHIDPlugin.instance.Config.mhid_charge_interval)
            {
                timers[__instance] = 0f;
                if (__instance.curItem != ItemType.MicroHID)
                {
                    for (int i = 0; i < __instance.items.Count; i++)
                    {
                        if (__instance.items[i].id == ItemType.MicroHID)
                        {
                            var item = __instance.items[i];
                            item.durability += MicroHIDPlugin.instance.Config.mhid_charge_rate;
                            __instance.items[i] = item;
                        }
                    }
                }
            }
        }
    }
}
