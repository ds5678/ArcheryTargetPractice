using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace ArcheryTargetPractice;

[HarmonyPatch(typeof(ArrowItem), nameof(ArrowItem.HandleCollisionWithObject))]
internal class ArcheryTargetPracticePatch
{
	private static void Prefix(ArrowItem __instance, GameObject collider, Vector3 collisionPoint)
	{
		// Must hit the target
		if (collider.name != "OBJ_BullseyeTarget_Prefab") return;

		// must be level 0 or 1 (1 or 2 in game)
		int archerylevel = GameManager.GetSkillArchery().GetCurrentTierNumber();
		if (archerylevel > 1) return;

		// Must be standing a decent distance away
		var timeInAir = Time.time - __instance.m_ReleaseTime;
		if (timeInAir < 0.25) return;

		// Must hit the paper bullseye (not the outer rim)
		if (collisionPoint.x < 1646.7 || collisionPoint.x > 1647.2) return;
		if (collisionPoint.y < 43.9 || collisionPoint.y > 44.7) return;
		if (collisionPoint.z < 1827.9 || collisionPoint.z > 1828.6) return;

		GameManager.GetSkillsManager().IncrementPointsAndNotify(SkillType.Archery, 1, SkillsManager.PointAssignmentMode.AssignOnlyInSandbox);
	}
}
