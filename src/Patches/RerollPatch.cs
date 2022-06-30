using HarmonyLib;
using flanne.Core;
using flanne;
using UnityEngine;
using UnityEngine.UI;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    static class RerollPatch
    {
        static Button reroolButton;
        static bool isShanaPlaying;

        [HarmonyPatch(typeof(InitState), "Enter")]
        [HarmonyPrefix]
        static void InitStateEnter_prefix(ref PowerupMenuState __instance, GameController ___owner)
        {
            reroolButton = ((Button)Traverse.Create(__instance).Property("powerupRerollButton").GetValue());
            isShanaPlaying = Loadout.CharacterSelection.name == "Shana";
            if (!DebugUtilityPlugin.PatchEnabled(DebugUtilityPlugin.hasInfiniteReroll)) return;

            // Set reroll button active to give the reroll passive to every character
            PowerupGenerator.CanReroll = true;
        }

        [HarmonyPatch(typeof(PowerupMenuState), "OnReroll")]
        [HarmonyPostfix]
        static void OnReroll_postfix(ref PowerupMenuState __instance)
        {
            if (!DebugUtilityPlugin.PatchEnabled(DebugUtilityPlugin.hasInfiniteReroll)) return;

            // Set reroll button active after reroll, to obtain infinite reroll
            reroolButton.gameObject.SetActive(true);
        }

        public static void ChangePatch()
        {
            PowerupGenerator.CanReroll = DebugUtilityPlugin.hasInfiniteReroll.Value || isShanaPlaying;
        }
    }
}
