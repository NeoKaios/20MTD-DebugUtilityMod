using HarmonyLib;
using flanne;
using flanne.Player;
using flanne.Core;
using UnityEngine;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    class UpdateWithPausePatch
    {
        [HarmonyPatch(typeof(PauseState), "Exit")]
        [HarmonyPrefix]
        static void PauseStateExit_prefix(ref PauseState __instance)
        {
            Debug.Log("Pause exit");
            GunPatch.SetInfiniteAmmo(DebugUtilityPlugin.hasGunPatch.Value);
        }
    }
}
