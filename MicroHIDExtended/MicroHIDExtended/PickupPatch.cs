using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EXILED;
using EXILED.Extensions;
using Harmony;
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
                if (timers[__instance] >= MicroHIDPlugin.chargeIntervals)
                {
                    timers[__instance] = 0f;
                    var info = __instance.info;
                    info.durability += MicroHIDPlugin.chargeRate;
                    __instance.SetupPickup(info);
                    __instance.RefreshDurability();
                }
            }
        }
    }
}
