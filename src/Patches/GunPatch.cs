using HarmonyLib;
using flanne;
using UnityEngine;

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
            SetInfiniteAmmo(DebugUtilityPlugin.PatchEnabled(DebugUtilityPlugin.hasGunPatch));
        }

        public static void SetInfiniteAmmo(bool isInfinite)
        {
            if (isInfinite == infiniteAmmo.value) return; // No change
            // Change
            if (isInfinite)
                infiniteAmmo.Flip();
            else
                infiniteAmmo.UnFlip();
        }
    }
}
