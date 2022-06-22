using HarmonyLib;
using flanne.Core;
using flanne.Player;
using flanne;

namespace DebugUtilityMod
{
    static class FastXPPatch
    {
        [HarmonyPatch(typeof(PlayerXP), "Awake")]
        [HarmonyPostfix]
        static void PlayerXPAwake_postfix(ref StatMod ___xpMultiplier)
        {
            // Add multiplier to XP amount gain, value stored in config (v0.1)
            ___xpMultiplier.AddMultiplierBonus(DebugUtilityPlugin.XPmult.Value);
        }
    }
}
