using HarmonyLib;
using flanne.Core;
using flanne;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    class InvincibilityPatch
    {
        [HarmonyPatch(typeof(InitState), "Exit")]
        [HarmonyPostfix]
        static void InitStateExit_postfix(ref InitState __instance)
        {
            if (!DebugUtilityPlugin.PatchEnabled(DebugUtilityPlugin.hasInvincibility)) return;

            //Flip invincibility bool
            ((Health)Traverse.Create(__instance).Property("playerHealth").GetValue()).isInvincible.Flip();
        }
    }
}
