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
        static bool currentState = false; // Necessary due to run that starts with HolyShield on

        [HarmonyPatch(typeof(InitState), "Exit")]
        [HarmonyPostfix]
        static void InitStateExit_postfix(ref InitState __instance)
        {
            currentState = false;
            toggleData = ((Health)Traverse.Create(__instance).Property("playerHealth").GetValue()).isInvincible;
            ChangePatch();
        }

        public static void ChangePatch()
        {
            if (toggleData == null) return;
            bool isInvicible = DebugUtilityPlugin.hasInvincibility.Value;
            if (isInvicible == currentState) return; // No change
            // Change
            DebugUtilityPlugin.ProgressionAllowed();
            currentState = isInvicible;
            if (isInvicible)
                toggleData.Flip();
            else
                toggleData.UnFlip();
        }
    }
}
