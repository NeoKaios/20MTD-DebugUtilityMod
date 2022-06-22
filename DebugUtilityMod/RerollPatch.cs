using HarmonyLib;
using flanne.Core;
using UnityEngine.UI;

namespace DebugUtilityMod
{
    static class RerollPatch
    {
        [HarmonyPatch(typeof(PowerupMenuState), "Enter")]
        [HarmonyPostfix]
        static void Enter_postfix(ref PowerupMenuState __instance)
        {
            // Set reroll button active to give the reroll passive to every character
            ((Button)Traverse.Create(__instance).Property("powerupRerollButton").GetValue()).gameObject.SetActive(true);
        }

        [HarmonyPatch(typeof(PowerupMenuState), "OnReroll")]
        [HarmonyPostfix]
        static void OnReroll_postfix(ref PowerupMenuState __instance)
        {
            // Set reroll button active after reroll, to obtain infinite reroll
            ((Button)Traverse.Create(__instance).Property("powerupRerollButton").GetValue()).gameObject.SetActive(true);
        }
    }
}
