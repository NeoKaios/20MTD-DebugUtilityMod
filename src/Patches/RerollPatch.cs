using System;
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
        static void InitStateEnter_prefix(GameController ___owner)
        {
            if (!DUMPlugin.activateMod.Value) return;
            reroolButton = ___owner.powerupRerollButton;
            isShanaPlaying = Loadout.CharacterSelection.name == "Shana";
            DUMPlugin.hasInfiniteReroll.SettingChanged += ChangePatch;
            // Set reroll button active to give the reroll passive to every character
            if (DUMPlugin.hasInfiniteReroll.Value) PowerupGenerator.CanReroll = true;
        }

        [HarmonyPatch(typeof(PowerupMenuState), "OnReroll")]
        [HarmonyPostfix]
        static void OnReroll_postfix(ref PowerupMenuState __instance)
        {
            if (!DUMPlugin.hasInfiniteReroll.Value) return;

            // Set reroll button active after reroll, to obtain infinite reroll
            reroolButton.gameObject.SetActive(true);
        }

        public static void ChangePatch(object sender, EventArgs e)
        {
            PowerupGenerator.CanReroll = DUMPlugin.hasInfiniteReroll.Value || isShanaPlaying;
            NoUnlockPatch.SetProgressionForbidden();
        }
    }
}
