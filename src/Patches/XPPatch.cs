using HarmonyLib;
using flanne.Core;
using flanne.Player;
using flanne;

namespace DebugUtilityMod
{
    static class XPPatch
    {
        static bool doGainXP = true;
        static int lvl = 1;
        [HarmonyPatch(typeof(PlayerXP), "Awake")]
        [HarmonyPostfix]
        static void PlayerXPAwake_postfix(ref StatMod ___xpMultiplier)
        {
            // Add multiplier to XP amount gain, value stored in config (v0.1)
            ___xpMultiplier.AddMultiplierBonus(DebugUtilityPlugin.XPmult.Value - 1);
            lvl = 1;
            doGainXP = true;
        }


        [HarmonyPatch(typeof(PlayerXP), "GainXP")]
        [HarmonyPrefix]
        static bool GainXP_prefix()
        {
            return doGainXP;
        }
            
        [HarmonyPatch(typeof(CombatState), "OnLevelUP")]
        [HarmonyPrefix]
        static void OnLevelUp_prefix()
        {
            lvl++;
            doGainXP = lvl < DebugUtilityPlugin.maxPlayerLevel.Value;
        }
    }
}
