using HarmonyLib;
using flanne.Core;
using flanne;
using UnityEngine.UI;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    static class RerollPatch
    {
        static Button reroolButton;
        [HarmonyPatch(typeof(InitState), "Enter")]
        [HarmonyPrefix]
        static void InitStateEnter_prefix(ref PowerupMenuState __instance)
        {
            if (!DebugUtilityPlugin.activateMod.Value || !DebugUtilityPlugin.hasInfiniteReroll.Value) return;

            // Set reroll button active to give the reroll passive to every character
            //((Button)Traverse.Create(__instance).Property("powerupRerollButton").GetValue()).gameObject.SetActive(true);
            PowerupGenerator.CanReroll = true;
            reroolButton = ((Button)Traverse.Create(__instance).Property("powerupRerollButton").GetValue());
        }

        [HarmonyPatch(typeof(PowerupMenuState), "OnReroll")]
        [HarmonyPostfix]
        static void OnReroll_postfix(ref PowerupMenuState __instance)
        {
            if (!DebugUtilityPlugin.activateMod.Value || !DebugUtilityPlugin.hasInfiniteReroll.Value) return;

            // Set reroll button active after reroll, to obtain infinite reroll
            reroolButton.gameObject.SetActive(true);
        }
    }
}
