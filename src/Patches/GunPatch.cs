using HarmonyLib;
using flanne;
using BepInEx.Configuration;
using UnityEngine;
using System.Collections.Generic;

namespace DebugUtilityMod
{
    [HarmonyPatch]
    class GunPatch
    {
        static BoolToggle infiniteAmmo = null;

        [HarmonyPatch(typeof(Ammo), "Start")]
        [HarmonyPostfix]
        static void AmmoStart_postfix(ref Ammo __instance)
        {
            infiniteAmmo = __instance.infiniteAmmo;
            ChangePatch();
        }

        public static void ChangePatch()
        {
            if (infiniteAmmo == null) return;
            bool isInfinite = DebugUtilityPlugin.hasGunPatch.Value;
            if (isInfinite == infiniteAmmo.value) return; // No change
            // Change
            DebugUtilityPlugin.ProgressionAllowed();
            if (isInfinite)
                infiniteAmmo.Flip();
            else
                infiniteAmmo.UnFlip();
        }
    }
}
