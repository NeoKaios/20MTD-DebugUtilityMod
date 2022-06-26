using HarmonyLib;
using flanne.Core;
using flanne;

namespace DebugUtilityMod
{
    class InvincibilityPatch
    {
        [HarmonyPatch(typeof(CombatState), "Enter")]
        [HarmonyPostfix]
        static void CombatStateEnter_postfix(ref CombatState __instance)
        {
            //Flip invincibility bool
            ((Health)Traverse.Create(__instance).Property("playerHealth").GetValue()).isInvincible.Flip();
        }
    }
}
