using HarmonyLib;
using flanne.Core;
using flanne;
using UnityEngine;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    class InvincibilityPatch
    {
        static BoolToggle toggleData;

        [HarmonyPatch(typeof(InitState), "Exit")]
        [HarmonyPostfix]
        static void InitStateExit_postfix(ref InitState __instance)
        {
            toggleData = ((Health)Traverse.Create(__instance).Property("playerHealth").GetValue()).isInvincible;
            ChangePatch();
        }

        public static void ChangePatch()
        {
            if (toggleData == null) return;
            bool isInvicible = DebugUtilityPlugin.hasInvincibility.Value;
            if (isInvicible == toggleData.value) return; // No change
            // Change
            DebugUtilityPlugin.ProgressionAllowed();
            if (isInvicible)
                toggleData.Flip();
            else
                toggleData.UnFlip();
        }
    }
}
