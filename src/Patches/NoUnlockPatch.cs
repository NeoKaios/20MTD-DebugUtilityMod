using HarmonyLib;
using flanne.Core;
using flanne;
using TMPro;
using flanne.UI;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    class NoUnlockPatch
    {
        [HarmonyPatch(typeof(PlayerSurvivedState), "CheckDifficultyUnlock")]
        [HarmonyPrefix]
        static bool CheckDifficultyUnlock_prefix()
        {
            return DebugUtilityPlugin.ProgressionAllowed();
        }

        [HarmonyPatch(typeof(PlayerSurvivedState), "CheckAchievmentUnlocks")]
        [HarmonyPrefix]
        static bool CheckAchievmentUnlocks_prefix()
        {
            return DebugUtilityPlugin.ProgressionAllowed();
        }

        [HarmonyPatch(typeof(ScoreCalculator), "GetScore")]
        [HarmonyPostfix]
        static void GetScore_postfix(ref Score __result)
        {
            // Modify score return value so that run with debug mode don't give any soul
            if (DebugUtilityPlugin.ProgressionAllowed()) return;

            __result.enemiesKilledScore = 0;
            __result.levelsEarnedScore = 0;
            __result.timeSurvivedScore = 0;
        }

        [HarmonyPatch(typeof(EndScreenUIC), "SetScores")]
        [HarmonyPostfix]
        static void SetScores_postfix(ref TMP_Text ___totalScoreTMP)
        {
            if (DebugUtilityPlugin.ProgressionAllowed()) return;

            ___totalScoreTMP.text = "DUM is/was active   " + ___totalScoreTMP.text;

        }
    }
}
