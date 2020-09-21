using HarmonyLib;
using Mirror;
using UnityEngine;

namespace MicroHIDExtended
{
	[HarmonyPatch(typeof(MicroHID), "DealDamage")]
	class DealDamagePatch
	{
		public static void Prefix(MicroHID __instance)
		{
			if (MicroHIDPlugin.instance.Config.mhid_damage_per_second == -1)
				return;
			__instance.damagePerSecond = MicroHIDPlugin.instance.Config.mhid_damage_per_second;
		}
	}
}
