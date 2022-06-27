using HarmonyLib;
using flanne.Player;
using flanne;
using UnityEngine;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    class GunPatch
    {

        [HarmonyPatch(typeof(Ammo), "Start")]
        [HarmonyPostfix]
        static void AmmoStart_postfix(ref Ammo __instance)
        {
            if (!DebugUtilityPlugin.activateMod.Value || !DebugUtilityPlugin.hasGunPatch.Value) return;

            __instance.infiniteAmmo.Flip();
        }
    }
}
