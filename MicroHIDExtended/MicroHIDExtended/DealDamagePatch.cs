using HarmonyLib;
using Mirror;
using UnityEngine;

namespace MicroHIDExtended
{
	[HarmonyPatch(typeof(MicroHID), "DealDamage")]
	class DealDamagePatch
	{
		public static bool Prefix(MicroHID __instance)
		{
			if (!NetworkServer.active)
			{
				return false;
			}

			var baseObj = (NetworkBehaviour)__instance;

			__instance.damagePause += Time.deltaTime;
			if (__instance.damagePause < 0.2f)
			{
				return false;
			}
			__instance.damagePause -= 0.2f;
			global::ReferenceHub referenceHub = __instance.refHub;
			__instance.beamStart.localRotation = Quaternion.Euler(referenceHub.playerMovementSync.Rotations.x, 0f, 0f);
			foreach (GameObject gameObject in global::PlayerManager.players)
			{
				if (gameObject != null && gameObject != baseObj.gameObject && Vector3.Distance(baseObj.transform.position, gameObject.transform.position) <= __instance.beamLength)
				{
					Vector3 normalized = (__instance.beamStart.position - gameObject.transform.position).normalized;
					RaycastHit raycastHit;
					if (Vector3.Dot(__instance.beamStart.forward, normalized) < -0.95f && Physics.Raycast(new Ray(__instance.beamStart.position, -normalized), out raycastHit, __instance.beamLength, __instance.beamMask))
					{
						if (raycastHit.collider != __instance.lastVictim)
						{
							__instance.lastVictim = raycastHit.collider;
							__instance.lastVictimRefs = raycastHit.collider.GetComponentInParent<global::ReferenceHub>();
						}
						global::ReferenceHub referenceHub2 = __instance.lastVictimRefs;
						if (referenceHub2 != null && referenceHub2.gameObject != baseObj.gameObject && referenceHub.weaponManager.GetShootPermission(referenceHub2.characterClassManager, false))
						{
							int damage = 0;
							if (MicroHIDPlugin.instance.Config.mhid_damage_per_hit == -1)
							{
								float num = UnityEngine.Random.Range(-__instance.maxDamageVariationPercent, __instance.maxDamageVariationPercent) / 100f * __instance.damagePerSecond;
								damage = Mathf.RoundToInt((__instance.damagePerSecond + num) * 0.2f);
							}
							else
							{
								damage = MicroHIDPlugin.instance.Config.mhid_damage_per_hit;
							}
							bool isAchievement = referenceHub2.characterClassManager.CurRole.team == global::Team.SCP;
							if (!referenceHub.playerStats.HurtPlayer(new global::PlayerStats.HitInfo((float)damage, referenceHub.LoggedNameFromRefHub(), global::DamageTypes.MicroHid, referenceHub.queryProcessor.PlayerId), gameObject, false))
							{
								isAchievement = false;
							}
							__instance.TargetSendHitmarker(isAchievement);
						}
					}
				}
			}
			RaycastHit raycastHit2;
			if (Physics.Raycast(new Ray(__instance.beamStart.position, __instance.beamStart.forward), out raycastHit2, __instance.beamLength, __instance.beamMask))
			{
				global::BreakableWindow componentInParent = raycastHit2.collider.GetComponentInParent<global::BreakableWindow>();
				if (componentInParent != null && !componentInParent.isBroken)
				{
					float num3 = UnityEngine.Random.Range(-__instance.maxDamageVariationPercent, __instance.maxDamageVariationPercent) / 100f * __instance.damagePerSecond;
					int num4 = Mathf.RoundToInt((__instance.damagePerSecond + num3) * 0.2f);
					componentInParent.ServerDamageWindow((float)num4);
				}
			}

			return false;
		}
	}
}
